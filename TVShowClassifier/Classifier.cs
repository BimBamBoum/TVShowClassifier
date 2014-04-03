using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.Permissions;

namespace TVShowClassifier
{
    public class Classifier
    {
        #region Methods

        public void ClassifyTvShows(Configuration _config)
        {
            // Get all the new files
            ArrayList files = GetAllVideoFiles(_config);

            // Iterate through all the files
            foreach (FileInfo file in files)
            {
                TVShow show = FindShowFromFilename(_config, file.Name);
                if (show != null)
                {
                    // Build destination path
                    int season = FindSeasonFromFilename(file.Name);
                    String destinationPath = _config.m_tvShowDirectory + "\\" + show.m_folder;
                    if (season > 0)
                    {
                        destinationPath += "\\Season " + season;
                    }

                    // Create the directory if needed
                    if (!Directory.Exists(destinationPath))
                    {
                        Directory.CreateDirectory(destinationPath);
                        Program.LogMessageToFile("Creating directory [" + destinationPath + "]");
                    }

                    // Copy the file
                    try
                    {
                        String destinationFile = Path.Combine(destinationPath, file.Name);
                        bool deleteOldFile = false;

                        if (File.Exists(destinationFile))
                        {
                            FileInfo existingFile = new FileInfo(destinationFile);
                            if (existingFile.Length == file.Length)
                            {
                                Program.LogMessageToFile("File [" + file.Name + "] already exist in directory [" + destinationPath + "]");
                                deleteOldFile = true;
                            }
                        }
                        else
                        {
                            Program.LogMessageToFile("Copying file [" + file.Name + "] to directory [" + destinationPath + "]");
                            Program.ShowNotificationOnTrayIcon("Copying file [" + file.Name);

                            FileInfo newFile = file.CopyTo(destinationFile);
                            if (newFile.Length == file.Length)
                            {
                                Program.LogMessageToFile("File [" + file.Name + "] successfully copied in directory [" + destinationPath + "]");
                                deleteOldFile = true;
                            }
                        }

                        if (deleteOldFile)
                        {
                            Program.LogMessageToFile("Deleting original file [" + file.Name + "]");
                            file.Delete();
                        }
                    }
                    catch (System.Exception e)
                    {
                        Program.LogMessageToFile(String.Format("Unable copy the file, {0}", e.ToString()));
                        Program.ShowNotificationOnTrayIcon(String.Format("Unable copy the file, {0}", e.ToString()));
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------

        private TVShow FindShowFromFilename(Configuration _config, String _filename)
        {
            foreach (TVShow show in _config.m_tvShows)
            {
                Regex regex = new Regex(show.m_searchSequence, RegexOptions.IgnoreCase);
                if (regex.IsMatch(_filename))
                {
                    return show;
                }
            }

            Program.LogMessageToFile("Can't find a math in the TVShowList for file [" + _filename + "]");
            return null;
        }

        //--------------------------------------------------------------------------------

        private int FindSeasonFromFilename(String _fileName)
        {
            Regex regex = new Regex(".*s([0-9][0-9]).*", RegexOptions.IgnoreCase);
            Match match = regex.Match(_fileName);
            if (match.Success && match.Groups.Count > 1)
            {
                Group group = match.Groups[1];
                return Convert.ToInt32(group.ToString());
            }

            return 0;
        }

        //--------------------------------------------------------------------------------

        private ArrayList GetAllVideoFiles(Configuration _config)
        {
            ArrayList files = new ArrayList();

            if (Directory.Exists(_config.m_torrentSrcDirector))
            {
                try
                {
                    string[] extFilter = _config.m_fileExtensions.Split(new char[] { ',' });
                    DirectoryInfo dirInfo = new DirectoryInfo(_config.m_torrentSrcDirector);

                    //loop through each extension in the filter
                    foreach (string extension in extFilter)
                    {
                        files.AddRange(dirInfo.GetFiles(extension, SearchOption.AllDirectories));
                    }
                }
                catch (System.Exception e)
                {
                    Program.LogMessageToFile(String.Format("Unable retrive the incoming files, {0}", e.ToString()));
                }
            }
            else
            {
                Program.LogMessageToFile("Torrent search path not found (" + _config.m_torrentSrcDirector + ").");
            }

            return files;
        }

        #endregion
    }
}
