namespace NotesApp
{
    using System;
    using System.Xml;
    using System.Diagnostics;

    public class StartUp
    {
        private static string NoteDirectory =
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Notes\";

        static void Main()
        {
            ReadCommand();
            Console.ReadLine();
        }

        private static void ReadCommand()
        {

            Console.Write(Directory.GetDirectoryRoot(NoteDirectory));
            string Command = Console.ReadLine();

            switch (Command.ToLower())
            {

                case "new":
                    NewNote();
                    Main();
                    break;
                case "edit":
                    EditNote();
                    Main();
                    break;
                case "read":
                    ReadNote();
                    Main();
                    break;
                case "delete":
                    DeleteNote();
                    Main();
                    break;
                case "shownotes":
                    ShowNotes();
                    Main();
                    break;
                case "dir":
                    NotesDirectory();
                    Main();
                    break;
                case "cls":
                    Console.Clear();
                    Main();
                    break;
                case "exit":
                    Exit();
                    break;
                default:
                    CommandsAvailable();
                    Main();
                    break;
            }
        }

        private static void NewNote()
        {
            Console.WriteLine("Please Enter Note:\n");
            string input = Console.ReadLine();

            XmlWriterSettings NoteSettings = new XmlWriterSettings
            {
                CheckCharacters = false,
                ConformanceLevel = ConformanceLevel.Auto,
                Indent = true
            };

            string FileName = DateTime.Now.ToString("dd-MM-yy") + ".xml";

            using XmlWriter NewNote = XmlWriter.Create(NoteDirectory + FileName, NoteSettings);
            NewNote.WriteStartDocument();
            NewNote.WriteStartElement("Note");
            NewNote.WriteElementString("body", input);
            NewNote.WriteEndElement();

            NewNote.Flush();
            NewNote.Close();
        }
        private static void EditNote()
        {
            Console.WriteLine("Please enter file name.\n");

            string FileName = Console.ReadLine().ToLower();

            if (File.Exists(NoteDirectory + FileName))
            {

                XmlDocument doc = new XmlDocument();

                try
                {
                    doc.Load(NoteDirectory + FileName);
                    Console.Write(doc.SelectSingleNode("//body").InnerText);
                    string ReadInput = Console.ReadLine();

                    if (ReadInput.ToLower() == "cancel")
                    {
                        Main();
                    }
                    else
                    {
                        string newText = doc.SelectSingleNode("//body").InnerText = ReadInput;
                        doc.Save(NoteDirectory + FileName);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not edit note following error occurred: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("File not found\n");
            }
        }
        private static void ReadNote()
        {
            Console.WriteLine("Please enter file name.\n");
            string FileName = Console.ReadLine().ToLower();

            if (File.Exists(NoteDirectory + FileName))
            {
                XmlDocument Doc = new XmlDocument();
                Doc.Load(NoteDirectory + FileName);
                Console.WriteLine(Doc.SelectSingleNode("//body").InnerText);
            }
            else
            {
                Console.WriteLine("File not found");
            }
        }
        private static void DeleteNote()
        {
            Console.WriteLine("Please enter file name\n");
            string FileName = Console.ReadLine();

            if (File.Exists(NoteDirectory + FileName))
            {
                Console.WriteLine(Environment.NewLine + "Are you sure you wish to delete this file? Y/N\n");
                string Confirmation = Console.ReadLine().ToLower();

                if (Confirmation == "y")
                {
                    try
                    {
                        File.Delete(NoteDirectory + FileName);
                        Console.WriteLine("File has been deleted\n");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("File not deleted following error occured: " + ex.Message);
                    }
                }
                else if (Confirmation == "n")
                {
                    Main();
                }
                else
                {
                    Console.WriteLine("Invalid command\n");
                    DeleteNote();
                }
            }
            else
            {
                Console.WriteLine("File does not exist\n");
                DeleteNote();
            }
        }
        private static void ShowNotes()
        {
            string NoteLocation = NoteDirectory;
            DirectoryInfo Dir = new DirectoryInfo(NoteLocation);

            if (Directory.Exists(NoteLocation))
            {
                FileInfo[] NoteFiles = Dir.GetFiles("*.xml");

                if (NoteFiles.Count() != 0)
                {
                    Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop + 2);
                    Console.WriteLine("+------------+");
                    foreach (var item in NoteFiles)
                    {
                        Console.WriteLine("  " + item.Name);
                    }
                    Console.WriteLine(Environment.NewLine);
                }
                else
                {
                    Console.WriteLine("No notes found.\n");
                }
            }
            else
            {
                Console.WriteLine(" Directory does not exist.....creating directory\n");
                Directory.CreateDirectory(NoteLocation);
                Console.WriteLine(" Directory: " + NoteLocation + " created successfully.\n");
            }
        }
        private static void NotesDirectory()
        {
            Process.Start("explorer.exe", NoteDirectory);
        }
        private static void Exit()
        {
           Environment.Exit(0);
        }
        private static void CommandsAvailable()
        {
            Console.WriteLine(" New - Create a new note\n Edit - Edit a note\n Read -  Read a note\n ShowNotes - List all notes\n Exit - Exit the application\n Dir - Opens note directory\n Help - Shows this help message\n");
        }
    }
}