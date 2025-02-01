using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CogniCard.Firebase
{
    public class FirebaseSetup
    {
        const string CredentialFilePath = "./cognicard-f7136196c39d.json";

        // 🔥🔥🔥🔥🔥🔥🔥🔥🔥🔥
        public static void InitializeFirebase()
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(CredentialFilePath)
            });
        }
    }
}
