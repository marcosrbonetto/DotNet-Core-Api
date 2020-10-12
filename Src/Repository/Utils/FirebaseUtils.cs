using System;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;

namespace ApiDebts.Src.Repository.Utils
{
    public class FirebaseUtils
    {
        private static FirebaseUtils _instance;
        public static FirebaseUtils Instance
        {
            get
            {
                if (_instance == null) _instance = new FirebaseUtils();
                return _instance;

            }
        }

        private FirebaseUtils()
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(options: new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("Resources/firebaseAdminConfig.json"),
                });
            }
        }

        public Model.DTO.UserFirebase GetData(string tokenId)
        {
            FirebaseToken decodedToken = FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(tokenId).Result;
            UserRecord userInfo = FirebaseAuth.DefaultInstance.GetUserAsync(decodedToken.Uid).Result;
            return new Model.DTO.UserFirebase
            {
                UID = decodedToken.Uid,
                Name = GetNombre(decodedToken, userInfo),
                Email = GetEmail(decodedToken, userInfo),
                Picture = GetPicture(decodedToken, userInfo)
            };
        }


        private string GetEmail(FirebaseToken decodedToken, UserRecord userInfo)
        {
            try
            {
                string email = null;
                if (decodedToken.Claims.ContainsKey("email") && decodedToken.Claims["email"] != null && ((string)decodedToken.Claims["email"]).Trim() != "")
                {
                    email = (string)decodedToken.Claims["email"];
                }
                else
                {
                    if (userInfo.Email != null && userInfo.Email.Trim() != "")
                    {
                        email = userInfo.Email;
                    }
                    else
                    {
                        if (userInfo.CustomClaims.ContainsKey("email") && userInfo.CustomClaims["email"] != null && ((string)userInfo.CustomClaims["email"]).Trim() != "")
                        {
                            email = (string)userInfo.CustomClaims["email"];
                        }
                        else
                        {
                            foreach (var provider in userInfo.ProviderData)
                            {
                                if (provider.Email != null && provider.Email.Trim() != "")
                                {
                                    email = provider.Email;
                                    break;
                                }
                            }

                        }
                    }
                }

                if (email == null || email.Trim() == "")
                {
                    throw new Exception("No se pudo encontrar el email del usuario");
                }
                email = email.Trim().ToLower();
                return email;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string GetNombre(FirebaseToken decodedToken, UserRecord userInfo)
        {
            try
            {
                string name = null;
                if (decodedToken.Claims.ContainsKey("name"))
                {
                    name = (string)decodedToken.Claims["name"];
                }
                else
                {
                    if (userInfo.DisplayName != null)
                    {
                        name = userInfo.DisplayName;
                    }
                    else
                    {
                        if (userInfo.CustomClaims.ContainsKey("name"))
                        {
                            name = (string)userInfo.CustomClaims["name"];
                        }
                        else
                        {
                            foreach (var provider in userInfo.ProviderData)
                            {
                                if (provider.DisplayName != null)
                                {
                                    name = provider.DisplayName;
                                    break;
                                }
                            }

                        }
                    }
                }
                if (name == null || name.Trim() == "")
                {
                    throw new Exception("No se pudo encontrar el nombre del usuario");
                }
                name = name.Trim().ToLower();
                return name;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string GetPicture(FirebaseToken decodedToken, UserRecord userInfo)
        {
            try
            {
                string picture = null;
                if (decodedToken.Claims.ContainsKey("picture"))
                {
                    picture = (string)decodedToken.Claims["picture"];
                }
                else
                {
                    if (userInfo.PhotoUrl != null)
                    {
                        picture = userInfo.PhotoUrl;
                    }
                    else
                    {
                        if (userInfo.CustomClaims.ContainsKey("picture"))
                        {
                            picture = (string)userInfo.CustomClaims["picture"];
                        }
                        else
                        {
                            foreach (var provider in userInfo.ProviderData)
                            {
                                if (provider.PhotoUrl != null)
                                {
                                    picture = provider.PhotoUrl;
                                    break;
                                }
                            }

                        }
                    }
                }
                return picture;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}