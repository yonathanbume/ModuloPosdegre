using AKDEMIC.ENTITIES.Models.Laurassia;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class NewSeed
    {
        public static New[] Seed(AkdemicContext context)
        {
            var result = new List<New>()
            {
                new New {
                    URL = "https://akdemic.blob.core.windows.net/news/c8062a1c-2484-463e-b37c-896b3643d02f.png",
                    Name = "636573518879122483.jpg",
                    State = "ACT"
                }
            };
            return result.ToArray();
        }
    }
}
