using static System.Console;

class Program
{
    private static string path = null;// путь к файлу будет храниться тут

    private static string tryAgainText = ". Попробуйте еще раз";

    public static void ErrorMsg(string err)
    {
        ForegroundColor = ConsoleColor.Red;
        Write(err);
        ForegroundColor = ConsoleColor.Gray;
    }

    public static void WarningMsg(string text)
    {
        ForegroundColor = ConsoleColor.Yellow;
        Write(text);
        ForegroundColor = ConsoleColor.Gray;
    }

    public static void ResultMsg(string text)
    {
        ForegroundColor = ConsoleColor.Green;
        Write(text);
        ForegroundColor = ConsoleColor.Gray;
    }

    public static void TitleMsg(string text)
    {
        ForegroundColor = ConsoleColor.Blue;
        Write(text);
        ForegroundColor = ConsoleColor.Gray;
    }

    private static string GetPathFromConsole()
    {
        path = ReadLine();
        if (PathExists(path))
        {
            FileInfo fileInfo = new FileInfo(path);
        }
        else
        {
            return null;
        }

        return path;
    }

    private static bool PathExists(string path)
    {
        try
        {
            FileInfo fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                return false;
            }
        }
        catch
        {
            return false;
        }

        return true;
    }

    //1
    private static void Menu()
    {
        WriteLine("Введите путь к файлу");
        string filePath = GetPathFromConsole();
        bool doContinue = true;

        if (filePath != null)
        {
            CreateDuplicate(filePath);
            ChangeText(path, GetTextBackwards(GetString(path)));
            ResultMsg("Путь принят!\n");
        }
        else
        {
            ErrorMsg("Такого пути к файлу нет" + tryAgainText + "\n");
            Menu();
        }
    }

    // принимает путь к файлу и изменение - возвращает измененный путь
    private static string ChangePathToFile(string path, string change)
    {
        // получить родительский путь
        string dir = Path.GetDirectoryName(path);
        // получить имя
        string name = Path.GetFileNameWithoutExtension(path);
        // получить расширение
        string extention = Path.GetExtension(path);
        // скомбинировать путь + имя + новый текст + расширение и вернуть
        return Path.Combine(dir, name + change + extention);
    }

    // принимает путь к файлу - создает пустой файл
    private static void CreateAndFillFile(string path, string text)
    {
        using (StreamWriter sw = new StreamWriter(path))
        {
            sw.Write(text);
        }
    }

    // принимает путь к файлу - создает новый файл, проверяет существует ли уже такой файл, создает файл
    private static void CreateDuplicate(string path)
    {
        if (path != null)
        {
            string newPath = ChangePathToFile(path, " d"); // путь нового файла
            FileInfo fileInfo = new FileInfo(newPath);

            while (fileInfo.Exists) // если уже такой файл есть, то снова добавить ещё букву
            {
                newPath = ChangePathToFile(newPath, " d");
                fileInfo = new FileInfo(newPath);
            }
            // если не существует, то создать
            CreateAndFillFile(newPath, GetString(path));
            //File.Copy(path, newPath, false); //копировать 1-ый файл в 2-ой и не переписывать, если уже есть
        }
    }

    private static void ChangeText(string path, string text)
    {
        using (StreamWriter sw = new StreamWriter(path))
        {
            sw.Write(text);
        }
    }

    private static string GetString(string path) // получить содержимое файла
    {
        try
        {
            string[] lines = File.ReadAllLines(path);
            string str = null;

            int count = 0;

            foreach (string line in lines)
            {
                if (count == lines.Length - 1) // убираю лишний (последний) перенос строки
                {
                    str += line;
                }
                else
                {
                    str += line + "\n";
                }

                count++;
            }

            return str;
        }
        catch (FileNotFoundException)
        {
            return null;
        }
    }

    public static string GetTextBackwards(string text)
    {
        char[] Arr = text.ToCharArray();
        Array.Reverse(Arr);

        return new string(Arr);
    }

    public static void Main()
    {
        TitleMsg("Программа копирует исходный файл в новый, а текст исходного файла перезаписывает задом наперед" + "\n");
        Menu();
    }
}
