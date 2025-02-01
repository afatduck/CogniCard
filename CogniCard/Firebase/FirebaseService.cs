using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CogniCard.Model.Json;
using Firebase.Database;
using Firebase.Database.Query;

namespace CogniCard.Firebase
{
    public class FirebaseService
    {
        private const string FirebaseDictionaryName = "collections";
        private const string FirebaseURI = "https://cognicard-default-rtdb.europe-west1.firebasedatabase.app/";

        private readonly FirebaseClient _firebaseClient;

        public FirebaseService()
        {
            _firebaseClient = new FirebaseClient(FirebaseURI);
        }

        public async Task UploadCollection(CollectionJson collectionJson)
        {
            string key = Guid.NewGuid().ToString();

            await _firebaseClient
                .Child(FirebaseDictionaryName)
                .Child(key)
                .PutAsync(collectionJson);
        }

        public async Task<List<CollectionJson>> GetCollections()
        {
            List<CollectionJson> collections = [];

            try
            {
                var data = await _firebaseClient
                    .Child(FirebaseDictionaryName)
                    .OnceAsync<CollectionJson>();

                foreach (var item in data)
                {
                    collections.Add(item.Object);
                }
            } catch (FirebaseException) { }

            return collections;
        }
    }
}
