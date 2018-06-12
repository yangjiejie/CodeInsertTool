using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


public class CodeManager
{
    List<CodeClass> codes;
    List<AddedCodeInfo> classDatas;
    public CodeManager()
    {
        codes = new List<CodeClass>();
        classDatas = new List<AddedCodeInfo>();
    }

    public void AddClassData(AddedCodeInfo cd)
    {
        if(!classDatas.Contains(cd))
        {
            classDatas.Add(cd);
        }
    }

    public void ReadCode()
    {
        codes.Clear();
        for(int i = 0; i < classDatas.Count; i++)
        {
            ReadCodeIns(classDatas[i]);
        }
    }

    public void ReadCodeIns(AddedCodeInfo data)
    {
        var utf8WithBom = new System.Text.UTF8Encoding(true);
        StreamReader sr = new StreamReader(data.pathName, utf8WithBom);
        string str;
        List<string> lines = new List<String>();
        while((str  = sr.ReadLine()) != null)
        {
            lines.Add(str);
        }
        sr.Dispose();

        CodeClass cc = new CodeClass(data,lines);
        codes.Add(cc);

    }

    public void WriteCode()
    {
        for(int i = 0; i < codes.Count ; i++)
        {
            codes[i].WriteInFile();
        }
    }
}

