using System;
using System.Security.Cryptography;
using System.Text;

namespace MHSEC_G
{
    internal class Crypt
    {
        internal static byte[] Rehash(byte[] model)
        {
            //InputStream input = new ByteArrayInputStream(fileArray);
            byte[] header = new byte[64];
            Array.Copy(model, header, 64);
            byte[] fileData = new byte[789328];
            Array.Copy(model, 64, fileData, 0, 789328);

            // Calculate the SHA-1 of the data and split it into 5 parts.
            byte[] checksum;
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                checksum = sha1.ComputeHash(fileData);
            }

            for (int i = 0; i < 5; i++)
            {
                byte tmp;
                tmp = checksum[0 + i * 4];
                checksum[0 + i * 4] = checksum[3 + i * 4];
                checksum[3 + i * 4] = tmp;
                tmp = checksum[1 + i * 4];
                checksum[1 + i * 4] = checksum[2 + i * 4];
                checksum[2 + i * 4] = tmp;
            }

            // Combine all the data together and write the data to files.
            byte[] fileFinal = new byte[789392];
            Array.Copy(checksum, 0, header, 12, 20);
            Array.Copy(header,   0, fileFinal, 0, 64);
            Array.Copy(fileData, 0, fileFinal, 64, 789328);

            return fileFinal;
        }
    }
}