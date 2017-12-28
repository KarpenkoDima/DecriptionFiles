<Query Kind="Program">
  <Namespace>System.Security.Cryptography</Namespace>
  <Namespace>System.Runtime.InteropServices</Namespace>
</Query>

void Main()
{
	// Must be 64 bits, 8 bytes.
            // Distribute this key to the user who will decrypt this file.
            DESCryptoServiceProvider sSecretKey;

            // Get the Key for the file to Encrypt.
            sSecretKey = Class1.GenerateKey();

            // For additional security Pin the key.
            ///GCHandle gch = GCHandle.Alloc(sSecretKey.Key, GCHandleType.Pinned);
			string USERNAME = @"%USERNAME%";
			string[] allDirs = Directory.GetDirectories(USERNAME);
	string[] files = Directory.GetFiles(@"G:\Encrypted\BaseParus_08092016\");
	foreach (var element in files)
	{


		// Encrypt the file.        
		//Class1.EncryptFile(element, element+"1", sSecretKey);
	
	// Decrypt the file.
	Class1.DecryptFile(element,
	   element+1,
	   sSecretKey);
}
	//gch.Free();
}

// Define other methods and classes here
class Class1
{
	//  Call this function to remove the key from memory after use for security
	[System.Runtime.InteropServices.DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
	public static extern bool ZeroMemory(IntPtr Destination, int Length);

	// Function to Generate a 64 bits Key.
	public static DESCryptoServiceProvider GenerateKey()
	{
		// Create an instance of Symetric Algorithm. Key and IV is generated automatically.
		DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();

            // Use the Automatically generated key for Encryption. 
            return desCrypto;
        }

        public static void EncryptFile(string sInputFilename,
           string sOutputFilename,
          DESCryptoServiceProvider sKey)
        {
            FileStream fsInput = new FileStream(sInputFilename,
               FileMode.Open,
               FileAccess.ReadWrite, FileShare.None);

            /*FileStream fsEncrypted = new FileStream(sOutputFilename,
               FileMode.Create,
               FileAccess.Write);*/
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = Encoding.ASCII.GetBytes("aaaaaaaa");
            DES.IV = Encoding.ASCII.GetBytes("aaaaaaaa");
            ICryptoTransform desencrypt = DES.CreateEncryptor();
          

            byte[] bytearrayinput = new byte[fsInput.Length];
            fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
		fsInput.Position = 0;
		CryptoStream cryptostream = new CryptoStream(fsInput,
desencrypt,
CryptoStreamMode.Write);
		cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Close();
            fsInput.Close();
            //fsEncrypted.Close();
        }

        public static void DecryptFile(string sInputFilename,
           string sOutputFilename,
           DESCryptoServiceProvider sKey)
        {
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            //A 64 bit key and IV is required for this provider.
            //Set secret key For DES algorithm.
            DES.Key = Encoding.ASCII.GetBytes("aaaaaaaa");
            //Set initialization vector.
            DES.IV = Encoding.ASCII.GetBytes("aaaaaaaa");

            //Create a file stream to read the encrypted file back.
            FileStream fsread = new FileStream(sInputFilename,
               FileMode.Open,
               FileAccess.Read);
            //Create a DES decryptor from the DES instance.
            ICryptoTransform desdecrypt = DES.CreateDecryptor();
            //Create crypto stream set to read and do a 
            //DES decryption transform on incoming bytes.
            CryptoStream cryptostreamDecr = new CryptoStream(fsread,
			   desdecrypt,
			   CryptoStreamMode.Read);
		//Print the contents of the decrypted file.
		FileStream fsDecrypted = new FileStream(sOutputFilename, FileMode.Create);
		byte[] bytearrayinput = new byte[1024];
		int length;
		do
		{
			length = cryptostreamDecr.Read(bytearrayinput, 0, 1024);
			fsDecrypted.Write(bytearrayinput, 0, length);
		} while (length == 1024);
		fsDecrypted.Flush();
		fsDecrypted.Close();
	}
}