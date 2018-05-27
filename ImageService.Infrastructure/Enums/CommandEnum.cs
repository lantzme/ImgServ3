using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Infrastructure.Enums
{
    public enum CommandEnum
    {
        NewFileCommand,
        GetConfigCommand,
        GetLogsCommand,
        NewLog,
        LogCommand,
        CloseCommand,
        CloseDirectoryHandlerCommand,
        CloseClientConnection
    }
}
