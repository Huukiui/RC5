using System.Text;

namespace RC5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            RC5 rc = new RC5(16, 12, 16, "abc");
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"RC5 Encryption/Decryption Menu (Current Parameters: w={rc.w}, r={rc.r}, b={rc.b}, password=\"{rc.password}\"):");
                Console.WriteLine("1. Set parameters (w, b, r, password)");
                Console.WriteLine("2. Encrypt File");
                Console.WriteLine("3. Encrypt String");
                Console.WriteLine("4. Decrypt File");
                Console.WriteLine("5. Exit");
                Console.Write("Enter your choice: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        (int w, int r, int b, string? password) = GetParameters();
                        rc = new RC5(w, r, b, password);
                        break;
                    case "2":
                        EcnryptFile(rc);
                        break;
                    case "3":
                        EncryptString(rc);
                        break;
                    case "4":
                        DecryptFile(rc);
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        static (int w, int r, int b, string? password) GetParameters()
        {
            Console.Write("Enter w (word size in bits): ");
            int w = int.Parse(Console.ReadLine());
            while (!(w == 16 || w == 32 || w == 64))
            {
                Console.Write("Invalid word size. Try again: ");
                w = int.Parse(Console.ReadLine());
            }
            Console.Write("Enter b (key length in bytes): ");
            int b = int.Parse(Console.ReadLine());
            while (!(b == 8 || b == 16 || b == 32))
            {
                Console.Write("Invalid key length. Try again: ");
                b = int.Parse(Console.ReadLine());
            }
            Console.Write("Enter r (number of rounds): ");
            int r = int.Parse(Console.ReadLine());
            while(!(r >= RC5.MinRounds &&  r <= RC5.MaxRounds))
            {
                Console.Write("Invalid rounds count. Try again: ");
                r = int.Parse(Console.ReadLine());
            }
            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            return (w, r, b, password);
        }

        static void EcnryptFile(RC5 rc)
        {
            Console.WriteLine("Enter a file name to encrypt:");
            string inputName = Console.ReadLine();
            Console.WriteLine("Enter a file name to save encrypted string:");
            string fileName = Console.ReadLine();
            using (FileStream fs = new FileStream(inputName, FileMode.OpenOrCreate))
            {
                rc.EncryptDataCBCPad(fs, fileName);
            }
            Console.WriteLine("Press any key to return to main menu...");
            Console.ReadKey();
        }

        static void EncryptString(RC5 rc)
        {
            Console.WriteLine("Enter a string to encrypt:");
            string input = Console.ReadLine();
            Console.WriteLine("Enter a file name to save encrypted string:");
            string fileName = Console.ReadLine();
            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            Console.WriteLine("\nOriginal bytes:\n");
            rc.PrintBytes(inputBytes);
            using (MemoryStream memoryStream = new MemoryStream(inputBytes))
            {
                rc.EncryptDataCBCPad(memoryStream, fileName);
            }
            Console.WriteLine("Press any key to return to main menu...");
            Console.ReadKey();
        }

        static void DecryptFile(RC5 rc)
        {
            Console.WriteLine("Enter a file name to decrypt:");
            string inputName = Console.ReadLine();
            if (File.Exists(inputName))
            {
                byte[] result = rc.DecryptDataCBCPad(inputName);
                Console.WriteLine("\nDecrypted bytes:\n");
                rc.PrintBytes(result);

                string decryptedText = Encoding.UTF8.GetString(result);
                Console.WriteLine("\nDecrypted text:\n");
                Console.WriteLine(decryptedText);    
            }
            else
            {
                Console.WriteLine("File doesnt exist!");
            }
            Console.WriteLine("Press any key to return to main menu...");
            Console.ReadKey();
        }
    }
}
