﻿using System.Collections.Generic;

namespace TextNotesSearch
{
    class FileLineDict
    {
        Dictionary<string, List<string>> fileLineDicts = new Dictionary<string, List<string>>();

        public void AddLine(string fileFullPath, string line)
        {
            fileLineDicts[fileFullPath].Add(line);
        }
    }
}
