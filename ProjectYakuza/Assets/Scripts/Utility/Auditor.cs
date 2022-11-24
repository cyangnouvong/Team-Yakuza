using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace CS4455.Utility
{
    public class Auditor : MonoBehaviour
    {      
        private const string BUILD_DIR = "Build";
        private const string WINDOWS_DIR = "Windows";
        private const string OSX_DIR = "OSX";


        [SerializeField] private string targetUnityVersion = "2020.3.34f1";
        [SerializeField] private string assignmentCode = "m1";
        [SerializeField] private Text guiText;
        [SerializeField] private string lastName;
        [SerializeField] private string firstInitial;
        [SerializeField] private bool disableNameChecks = false;

        [SerializeField] private Text btnText;

        private Text text;
        private Canvas canvas;

        private string unityVersion;

        private void Awake()
        {
            text = GetComponent<Text>();

            canvas = GetComponentInParent<Canvas>();
        }

        // Start is called before the first frame update
        private void Start()
        {  
            text.text = GetAudit();

        }

        //private bool DoesPossiblyHiddenDirectoryExist(string path)
        //{
        //    try
        //    {
        //        if ((new DirectoryInfo(path).Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
        //        {
        //            // file is hidden
        //            return true;
        //        }
        //        else
        //        {
        //            // file is not hidden
        //            return true;
        //        }
        //    }
        //    catch (DirectoryNotFoundException)
        //    {         // file does not exist
        //    }

        //    return false;
        //}


        private string GetAudit()
        {
            List<string> auditErrors = new List<string>();

            try
            {

                //if (Application.isEditor)
                //{
                //    auditErrors.Add("Audit will not run in Editor.");

                //    return CombineAuditError();
                //}


                // lastNameValid allowed to have other capitalized letters for complex names
                string lastNameValid = "";
                var firstInitialValid = "";

                unityVersion = Application.unityVersion;

                //Debug.Log($"Unity version is: {unityVersion}");

                if (!unityVersion.Equals(targetUnityVersion))
                {
                    auditErrors.Add($"• Prj not built with target Unity version: {targetUnityVersion}");
                }

                if (!disableNameChecks)
                {
                    // Check for missing fields not populated by student
                    bool fieldsMissing = false;
                    if (string.IsNullOrEmpty(lastName))
                    {
                        auditErrors.Add($"• Missing 'Last Name' in Auditor Inspector settings!");
                        fieldsMissing = true;
                    }
                    if (string.IsNullOrEmpty(firstInitial))
                    {
                        auditErrors.Add($"• Missing 'First Initial' in Auditor Inspector settings!");
                        fieldsMissing = true;
                    }
                    if (fieldsMissing)
                    {
                        return CombineAuditError();
                    }

                    // Check for invalid fields incorrectly populated by student
                    bool fieldsInvalid = false;

                    // lastNameValid allowed to have other capitalized letters for complex names
                    lastNameValid = char.ToUpper(lastName[0]) + lastName.Substring(1);
                    firstInitialValid = firstInitial.ToUpper();

                    if (lastName != lastNameValid)
                    {
                        auditErrors.Add($"• Invalid 'Last Name' ('{lastName}' != '{lastNameValid}') in Auditor!");
                        fieldsInvalid = true;
                    }
                    if (firstInitial != firstInitialValid)
                    {
                        auditErrors.Add($"• Invalid 'First Initial' ('{firstInitial}' != '{firstInitialValid}') in Auditor!");
                        fieldsInvalid = true;
                    }
                    if (fieldsInvalid) { return CombineAuditError(); }

                    if (guiText != null)
                    {
                        if (guiText.text.Contains("George P. Burdell") ||
                            !guiText.text.ToLower().Contains(lastNameValid.ToLower()))
                        {
                            auditErrors.Add($"• GUI Text name has not been set correctly. Must contain provided last name. Currently: {guiText.text}");
                        }
                    }
                }

                // Check for invalid product name
                string productName = Application.productName;
                string productNameValid = $"{lastNameValid}_{firstInitialValid}_{assignmentCode}";
                if (productName != productNameValid)
                {
                    auditErrors.Add($"• Invalid Product Name ('{productName}' != '{productNameValid}') in prjSettings:Player!");
                }

                bool isOSX = Application.platform == RuntimePlatform.OSXPlayer;

                //auditErrors.Add($"platform:{Application.platform} thinks it's {isOSX}");

                DirectoryInfo dataDirectory = isOSX ? new DirectoryInfo(Application.dataPath).Parent : new DirectoryInfo(Application.dataPath);
                DirectoryInfo appDirectory = dataDirectory.Parent;
                DirectoryInfo buildDirectory = appDirectory.Parent;
                DirectoryInfo rootDirectory = buildDirectory.Parent;

                string appDirectoryName = appDirectory.Name;
                string appDirectoryNameValid = isOSX ? OSX_DIR : WINDOWS_DIR;// GetOperatingSystemDirectory();


                string exeName = isOSX ? $"{productNameValid}.app" : $"{productNameValid}.exe";
                string exePath = $"{appDirectory.FullName}/{exeName}";

                bool wasErr = false;
                if (!isOSX)
                {
                    if (!File.Exists(exePath))
                        wasErr = true;
                }
                else
                {
                    if (!Directory.Exists(exePath))
                        wasErr = true;
                }

                if (wasErr)
                {
                    auditErrors.Add($"• Exe on current platform should be: {exePath}");
                }

                string altExeName = isOSX ? $"{productNameValid}.exe" : $"{productNameValid}.app";
                string subPath = isOSX ? WINDOWS_DIR : OSX_DIR;
                string altExePath = $"{buildDirectory.FullName}/{subPath}/{altExeName}";

                wasErr = false;

                if (isOSX)
                {
                    if (!File.Exists(altExePath))
                        wasErr = true;
                }
                else
                {
                    if (!Directory.Exists(altExePath))
                        wasErr = true;
                }

                if (wasErr)
                {
                    auditErrors.Add($"• Exe on alt platform should be: {altExePath}");
                }

                //bool untested1 = File.Exists($"{appDirectory.FullName}/UNTESTED");
                //bool untested2 = File.Exists($"{buildDirectory.FullName}/{subPath}/UNTESTED");

                bool untested1 = WildcardFileExists($"{appDirectory.FullName}/", "UNTESTED*");
                bool untested2 = WildcardFileExists($"{buildDirectory.FullName}/{subPath}/", "UNTESTED*");

                int untestedCount = (untested1 ? 1 : 0) + (untested2 ? 1 : 0);

                if (untestedCount != 1)
                {
                    auditErrors.Add($"Found UNTESTED file {untestedCount} times. But expected one. Preferred build should not have UNTESTED file");
                }

                //switch (Application.platform)
                //{
                //    case RuntimePlatform.WindowsPlayer:
                //        appDirectoryNameValid = WINDOWS_DIR;
                //        break;
                //    case RuntimePlatform.OSXPlayer:
                //        isOSX = true;
                //        appDirectoryNameValid = $"{productNameValid}.app";
                //        break;
                //    default:
                //        appDirectoryNameValid = "";
                //        auditErrors.Add($"• Can't determine running platform. Exiting!");
                //        return CombineAuditError();
                //}


                string buildDirectoryName = buildDirectory.Name;
                string buildDirectoryNameValid = BUILD_DIR;
                string rootDirectoryName = rootDirectory.Name;
                string rootDirectoryNameValid = productNameValid;

                // Check for invalid directory names
                bool possibleStructureInvalid = false;
                if (appDirectoryName != appDirectoryNameValid)
                {
                    auditErrors.Add($"• Invalid App Directory Name ('{appDirectoryName}' != '{appDirectoryNameValid}')!");
                    possibleStructureInvalid = true;
                }
                if (buildDirectoryName != buildDirectoryNameValid)
                {
                    auditErrors.Add($"• Invalid Build Directory Name ('{buildDirectoryName}' != '{buildDirectoryNameValid}')!");
                    possibleStructureInvalid = true;
                }
                if (rootDirectoryName != rootDirectoryNameValid)
                {
                    auditErrors.Add($"• Invalid Root Directory Name ('{rootDirectoryName}' != '{rootDirectoryNameValid}')!");
                    possibleStructureInvalid = true;
                }

                // Catch-All for common errors caused by incorrect folder hierarchy
                if (possibleStructureInvalid)
                {
                    auditErrors.Add($"• Your folders may be organized incorrectly.");
                    auditErrors.Add($"\t• Root Directory detected as '{rootDirectory.FullName}'!");
                    auditErrors.Add($"\t• Build Directory detected as '{buildDirectory.FullName}'!");
                    auditErrors.Add($"\t• Ensure your build folders are organized as follows:");
                    auditErrors.Add($"\t\t•'{productNameValid}/{BUILD_DIR}/{WINDOWS_DIR}/{productNameValid}.exe'");
                    auditErrors.Add($"\t\t•'{productNameValid}/{BUILD_DIR}/{OSX_DIR}/{productNameValid}.app'");

                    // Return here because other errors could be fired erraneously due to this incorrect structure
                    return CombineAuditError();
                }

                // Check for critical project parts
                string assetsPath = $"{rootDirectory.FullName}/Assets";
                if (!Directory.Exists(assetsPath))
                { auditErrors.Add($"• Did not find Asset folder at expected location '{assetsPath}'!"); }

                string projectSettingsPath = $"{rootDirectory.FullName}/ProjectSettings";
                if (!Directory.Exists(projectSettingsPath))
                { auditErrors.Add($"• Did not find ProjectSettings folder at expected location '{projectSettingsPath}'!"); }

                string packagesPath = $"{rootDirectory.FullName}/Packages";
                if (!Directory.Exists(packagesPath))
                { auditErrors.Add($"• Did not find Packages folder at expected location '{packagesPath}'!"); }

                // Check for known unneeded directories
                string gitPath = $"{rootDirectory.FullName}/.git";

                //if (File.Exists(gitPath))
                if (Directory.Exists(gitPath))
                { auditErrors.Add($"• Found unneeded .git folder '{gitPath}'!"); }

                string libraryPath = $"{rootDirectory.FullName}/Library";
                //if (File.Exists(libraryPath))
                if (Directory.Exists(libraryPath))
                { auditErrors.Add($"• Found unneeded Library folder '{libraryPath}'!"); }

                string tempPath = $"{rootDirectory.FullName}/temp";
                //if (File.Exists(tempPath))
                if (Directory.Exists(tempPath))
                { auditErrors.Add($"• Found unneeded temp folder '{tempPath}'!"); }

                string objPath = $"{rootDirectory.FullName}/Obj";
                if (Directory.Exists(objPath))
                { auditErrors.Add($"• Found unneeded Obj folder '{objPath}'!"); }

                string logsPath = $"{rootDirectory.FullName}/Logs";
                if (Directory.Exists(logsPath))
                { auditErrors.Add($"• Found unneeded Logs folder '{logsPath}'!"); }

                // Check for missing readme
                string readmePath = $"{rootDirectory.FullName}/{productNameValid}_readme";
                string readmePathTxt = $"{readmePath}.txt";
                string readmePathMd = $"{readmePath}.md";
                if (!File.Exists(readmePathTxt) && !File.Exists(readmePathMd))
                {
                    auditErrors.Add($"• Did not find readme .txt or .md file at expected location '{readmePath}'!");
                }

                //if (auditErrors.Count > 0)
                //{
                //    var path = rootDirectory.FullName + "/audit.log";
                //    auditErrors.Add($"Will attempt to save log to: {path}");
                //    try
                //    {
                //        File.WriteAllLines(path, auditErrors);
                //    }
                //    catch (Exception)
                //    { }
                //}

                if (auditErrors.Count <= 0)
                { 
                    auditErrors.Add("No major issues found, but make sure");
                    auditErrors.Add("to fill out descriptive readme and");
                    auditErrors.Add("clear out any major clutter in root dir");
                    auditErrors.Add("such as mistargeted build.");
                }


                return CombineAuditError();

            }
            catch(Exception e)
            {
                auditErrors.Add("Unknown error occured during audit!");
                auditErrors.Add(e.ToString());
                return CombineAuditError();
            }

           

            string CombineAuditError() {
                var ret = $"Unity version: { unityVersion}\n";
                ret += string.Join($"\n", auditErrors);
                return ret;
            }
        }

        private static string GetOperatingSystemDirectory()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer: return WINDOWS_DIR;
                case RuntimePlatform.OSXPlayer: return OSX_DIR;
                default: return $"Invalid Platform: '{Application.platform}'";
            }
        }

        // use * as wildcard. ? may work as well.
        private bool WildcardFileExists(string dir, string fnPtrn)
        {
            string[] files = new string[0];
            try
            {
                files = Directory.GetFiles(dir, fnPtrn, System.IO.SearchOption.TopDirectoryOnly);
            }
            catch (Exception)
            { }

            return files.Length > 0;

        }



        private void Update()
        {
            if( (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) &&
                Input.GetKeyDown(KeyCode.O) )
            {
                ToggleAuditDisplay();
            }
        }


        public void ToggleAuditDisplay()
        {
            //canvas.enabled = !canvas.enabled;
            text.enabled = !text.enabled;

            if(btnText != null)
            {
                var bit = text.enabled ? "Hide" : "Show";
                btnText.text = bit + " Audit";
            }
        }

    }
}