using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public class AddedCodeInfo
{
    public string pathName;
    public string msgId;
    public List<string> codeString;  //code 
    public string gameType;
    public string partFunName;
    public int curWriteIndex = 0;
}


class Program
{


    static string getXXXGameLogicDotH(string part)
    {
        string str = "    virtual void on" + part + "(Player* pPlayer, const void* data, size_t len);";
        return str;
    }
    
    static string getXXXGameLogicCpp(string part,string gameType)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("void " + gameType +"Logic::on"+part+"(Player* pPlayer, const void* data, size_t len)");
        sb.AppendLine("{");
        sb.AppendLine("    IF_NOT_RETURN(pPlayer != NULL );");
        sb.AppendLine("    IF_NOT_RETURN(data != NULL);");
        sb.AppendLine("}");
        return sb.ToString();
    }

    static string getGameLogicDotHString(string part)
    {
        string str = "    virtual void on" + part + "(Player* pPlayer, const void* data, size_t len) {}";
        return str;
    }

    static string getRoomDotHString(string part)
    {
        string str = "    void on" + part + "(Player* pPlayer,const void* data,size_t len);";
        return str;
    }

    static string getRoomCppString(string part)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("void Room::on" + part + "(Player* pPlayer,const void* data,size_t len)");
        sb.AppendLine("{");
        sb.AppendLine("    IF_NOT_RETURN(NULL != m_pGameLogic);");
        sb.AppendLine("    IF_NOT_RETURN(pPlayer != NULL);");
        sb.AppendLine("    m_pGameLogic->on"+part + "(pPlayer, data,len);");
        sb.AppendLine("}");

        return sb.ToString();
    }


    static string getRoomMsgCppHandleString()
    {
        return "    RegMsgHandler("+ msgId+ " ,    &RoomMessage::MH_Room_" + partFunName +");";
    }

    static string getRoomMsgCppCodeString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("void RoomMessage::MH_Room_" + partFunName + "(Entity* entity, const void* data, size_t len)");
        sb.AppendLine("{");
        sb.AppendLine("    IF_NOT_RETURN(entity != NULL && data != NULL);");
        sb.AppendLine("    IF_NOT_RETURN(IsPlayer(entity));");
        sb.AppendLine("    Player* pPlayer = dynamic_cast<Player*>(entity);");
        sb.AppendLine("    IF_NOT_RETURN(NULL != pPlayer);");
        sb.AppendLine("    Room* pRoom = pPlayer->getCurRoom();");
        sb.AppendLine("    IF_NOT_RETURN(NULL != pRoom);");
        sb.AppendLine("    pRoom->on" + partFunName + "(pPlayer, data, len);");
        sb.AppendLine("}");
        return sb.ToString();
    }

    static string getRoomMsgDotHFile(string part)
    {
        return "    static void MH_Room_"+part+"(Entity* entity, const void* data, size_t len);";
    }



    static AddedCodeInfo  ConstructClassData(string fileName,List<string> codeString)
    {
        AddedCodeInfo tmpData = new AddedCodeInfo();
        tmpData.pathName = fileName;
        tmpData.msgId = msgId;
        tmpData.partFunName = partFunName;
        tmpData.codeString = codeString;
        tmpData.gameType = gameType;
        return tmpData;
    }


    static void consturctPartFunName()
    {
        int count = 0;
        for (int i = 0; i < msgId.Length; i++)
        {
            if (msgId[i] == '_')
            {
                ++count;
                if (count == 2)
                {
                    partFunName = msgId.Substring(i + 1);
                    break;
                }
            }
        }
        
    }

    static string msgId = "MsgId_Fish_DeleteFish";
    static string gameType = "FishGame";
    static string partFunName = "";
    static string logicServerPath = "";

    static void Main(string[] args)
    {
        //使用者只需要指定消息协议码 其他相关的代码自动生成
        if(args.Length > 0 )
        {
            gameType = args[0];
            msgId = args[1];
            logicServerPath = args[2];
        }
        consturctPartFunName();
        for(int i  =0 ; i < args.Length; i++)
        {
            Console.WriteLine(args[i]);
        }
       
        string[] path  = new string[]
        {
            logicServerPath + @"\RoomMessage.h",
            logicServerPath + @"\RoomMessage.cpp",
            logicServerPath + @"\Room.h",
            logicServerPath + @"\Room.cpp",
            logicServerPath + @"\GameLogic.h",
            logicServerPath + @"\" + gameType + "Logic.h",
            logicServerPath + @"\" + gameType + "Logic.cpp",
        };


        List<List<string>> listlistString = new List<List<string>>()
        {
            new List<String>(){getRoomMsgDotHFile(partFunName)},
            new List<String>(){ getRoomMsgCppHandleString(), getRoomMsgCppCodeString(),},
            new List<String>(){ getRoomDotHString(partFunName)},
            new List<String>(){ getRoomCppString(partFunName)},
            new List<String>(){getGameLogicDotHString(partFunName)},
            new List<String>(){getXXXGameLogicDotH(partFunName)},
            new List<String>(){ getXXXGameLogicCpp(partFunName,gameType)},
        };



        CodeManager codeManager = new CodeManager();

        for(int i = 0; i < path.Length; i++)
        {
            AddedCodeInfo data = ConstructClassData(path[i], listlistString[i]);
            codeManager.AddClassData(data);
        }

        codeManager.ReadCode();
        codeManager.WriteCode();
        Console.ReadKey();
    }
}

