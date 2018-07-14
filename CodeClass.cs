using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public class CodeClass
{
    public List<string> m_lines;
    public AddedCodeInfo m_cData;
    public string tag;
    public CodeClass(AddedCodeInfo cData,List<String> lines)
    {
        m_cData = cData;
        m_lines = lines;
        tag = "\\["+cData.gameType+"]";
    }
    public bool HasTag(string line,string tag)
    {
        bool hastag = false;
        for(int i = 0; i < line.Length; i++)
        {
            
            if(line[i] == '/' && (i+2) <line.Length && line[i+1] == '/' && line[i+2] == '[')
            {
               if(line.Contains(tag))
               {
                   return true;
               }
               else
               {
                   return false;
               }
            }
        }
        return false;
    }
    public void WriteInFile()
    {
        var utf8WithBom = new System.Text.UTF8Encoding(true);
        StreamWriter sw = new StreamWriter(m_cData.pathName, false, utf8WithBom);
        for(int i = 0; i < m_lines.Count; i++)
        {
            sw.WriteLine(m_lines[i]);

            //如果有标签 \\[GameType] 则 

            if (HasTag(m_lines[i],m_cData.gameType))
            {
                string tmpStr = m_cData.codeString[m_cData.curWriteIndex];
                if(tmpStr.Contains('\r') && tmpStr.Contains('\n'))
                {
                    tmpStr = tmpStr.Substring(0, tmpStr.IndexOf('\r'));
                }
                if (!m_lines.Contains(tmpStr))
                {
                    sw.WriteLine(m_cData.codeString[m_cData.curWriteIndex++]);
                }
            }
        }
        sw.Dispose();
    }
}

