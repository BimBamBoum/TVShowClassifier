using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;

namespace TVShowClassifier
{
    public class Configuration
    {
        public Configuration()
        {
            m_tvShows = new ArrayList();
        }

        #region Methods

        public void LoadConfiguration(String configFilePath)
        {
            // Load the config file
            XmlTextReader xmlReader = null;

            try
            {
                xmlReader = new XmlTextReader(configFilePath);

                // Read all the infos
                while (xmlReader.Read())
                {
                    xmlReader.MoveToContent();
                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        switch (xmlReader.Name)
                        {
                            case "TorrentSrcDirectory":
                                xmlReader.MoveToAttribute("path");
                                m_torrentSrcDirector = xmlReader.Value;
                                break;
                            case "TVShowDirectory":
                                xmlReader.MoveToAttribute("path");
                                m_tvShowDirectory = xmlReader.Value;
                                break;
                            case "FileExtensions":
                                xmlReader.MoveToAttribute("value");
                                m_fileExtensions = xmlReader.Value;
                                break;
                            case "TVShow":
                                TVShow tvShow = new TVShow();
                                ReadTVShowInfos(xmlReader, tvShow);
                                m_tvShows.Add(tvShow);
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Program.LogMessageToFile(String.Format("Unable to process configuration file, {0}", e.ToString()));
            }
            finally
            {
                if (xmlReader != null)
                    xmlReader.Close();
            }
        }

        private void ReadTVShowInfos(XmlReader _xmlReader, TVShow _tvShow)
        {
            _xmlReader.MoveToAttribute("name");
            _tvShow.m_name = _xmlReader.Value;

            _xmlReader.MoveToAttribute("searchSequence");
            _tvShow.m_searchSequence = _xmlReader.Value;

            _xmlReader.MoveToAttribute("folder");
            _tvShow.m_folder = _xmlReader.Value;
        }

        #endregion

        #region Properties

        public String       m_torrentSrcDirector { get; set; }
        public String       m_tvShowDirectory { get; set; }
        public String       m_fileExtensions { get; set; }
        public ArrayList    m_tvShows { get; set; }

        #endregion
    }
}
