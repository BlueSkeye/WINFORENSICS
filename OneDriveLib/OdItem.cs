using System;
using Microsoft.Graph;

namespace OneDriveLib
{
    /// <summary>This class describes a generic OneDrive item, either a file or a folder.</summary>
    public class OdItem
    {
        public static void CreateItem()
        {
            HttpClient client = Microsoft.Graph.GraphClientFactory.Create();

            if (null == client) {
                throw new Exception("GraphClientFactory.Create() returned null.");
            }
            else {
                throw new Exception("Client successfully created.");
            }

        }
    }
}
