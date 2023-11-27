using System;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.InstitutionalWelfareViewModels
{
    public class NutritionalRecordViewModel
    {
        public string DoctorView { get; set; }
        public Guid MedicalAppointmentId { get; set; }
        public Guid BackId { get; set; }
        public Guid StudentId { get; set; }
        //STUDENT INFORMATION
        public string FullName { get; set; }
        public string CareerName { get; set; }
        public int CurrentYearAcademy { get; set; }
        public int Age { get; set; }


        //GENERAL_INFORMATION
        public Guid? Id { get; set; }
        public decimal CurrentWeight { get; set; }
        public decimal UsualWeight { get; set; }
        public decimal Size { get; set; }
        public decimal BodyFatPercentage { get; set; }
        public decimal MediumArmCircumference { get; set; }
        public decimal WaistCircumference { get; set; }
        public decimal HipCircumference { get; set; }
        public decimal IMC { get; set; }
        public string DiabetesAntecedent { get; set; }
        public string TriglyceriesAntecedent { get; set; }
        public string ThyroidAntecedent { get; set; }
        public string HypertensionAntecedent { get; set; }
        public string OtherAntecedent { get; set; }
        public bool PhysicalActivity { get; set; }
        public string TypePhysicalActivity { get; set; }
        public string FrequencyPhysicalActivity { get; set; }
        public string DurationPhysicalActivity { get; set; }
        public string TimeStartedPhysicalActivity { get; set; }

        //FEEDING
        /////////GENERAL
        public int WholeMilk { get; set; }
        public int SkimMilk { get; set; }
        public int WholeYogurt { get; set; }
        public int SkimYogurt { get; set; }
        public int FreshCheese { get; set; }
        public int PasteurizedCheese { get; set; }
        public int Egg { get; set; }
        public int Beef { get; set; }
        public int ChickenMeat { get; set; }
        public int SheepMeat { get; set; }
        public int Fish { get; set; }
        public int SeaFood { get; set; }
        public int Sausage { get; set; }
        public int Ham { get; set; }
        public int PorkCheese { get; set; }
        public int Liver { get; set; }
        public int Tongue { get; set; }
        public int Tripe { get; set; }

        /////////VEGETABLES
        public int Pea { get; set; }
        public int Vainite { get; set; }
        public int Chard { get; set; }
        public int ChiliPepper { get; set; }
        public int Celery { get; set; }
        public int Aubergine { get; set; }
        public int Broccoli { get; set; }
        public int Onion { get; set; }
        public int Cauliflower { get; set; }
        public int Corn { get; set; }
        public int Spinach { get; set; }
        public int Lettuce { get; set; }
        public int Cabbage { get; set; }
        public int Beetraga { get; set; }
        public int Pumpkin { get; set; }
        public int Tomato { get; set; }
        public int Carrot { get; set; }

        /////////FRUITS
        public int Banana { get; set; }
        public int Plum { get; set; }
        public int Damascus { get; set; }
        public int Peach { get; set; }
        public int Strawberry { get; set; }
        public int Grape { get; set; }
        public int Fig { get; set; }
        public int Kiwi { get; set; }
        public int Tangerine { get; set; }
        public int Orange { get; set; }
        public int Pear { get; set; }
        public int Watermelon { get; set; }
        public int Avocado { get; set; }
        public int Coconut { get; set; }
        public int Nuts { get; set; }


        /////////CEREALS
        public int TorataBread { get; set; }
        public int CrownBread { get; set; }
        public int FrenchBread { get; set; }
        public int WholemealBread { get; set; }
        public int Rice { get; set; }
        public int WhiteRice { get; set; }
        public int IntegralRice { get; set; }
        public int Paste { get; set; }
        public int Pizza { get; set; }
        public int Quinoa { get; set; }
        public int Wheat { get; set; }
        public int Cañihua { get; set; }

        /////////COOKIES
        public int WaterCookie { get; set; }
        public int SweetCookie { get; set; }
        public int SaltyCookie { get; set; }
        public int SweetFillingCookie { get; set; }

        /////////PASTRIES_AND_DESSERTS
        public int OrangeCake { get; set; }
        public int ChocolateCake { get; set; }
        public int DecoratedCake { get; set; }
        public int ThreeMilkCake { get; set; }
        public int Pionono { get; set; }
        public int Huarwero { get; set; }
        public int Strudel { get; set; }
        public int RicePudding { get; set; }
        public int MazamorraEnvelope { get; set; }
        public int Pudding { get; set; }
        public int IceCream { get; set; }

        /////////LEGUMBRES
        public int Bean { get; set; }
        public int Pallar { get; set; }
        public int Lentil { get; set; }
        public int DriedPeas { get; set; }
        public int DriedBeans { get; set; }
        public int Chickpea { get; set; }

        /////////SUGAR
        public int RefinedSugar { get; set; }
        public int Sweetener { get; set; }
        public int Jam { get; set; }
        public int Honey { get; set; }
        public int Chancaca { get; set; }
        public int Caramel { get; set; }
        public int Jelly { get; set; }

        /////////GRASAS
        public int VegatableOil { get; set; }
        public int OliveOil { get; set; }
        public int Butter { get; set; }
        public int Margarine { get; set; }
        public int MilkCream { get; set; }

        /////////ADEREZO
        public int Mayonnaise { get; set; }
        public int Mustard { get; set; }
        public int Ketchup { get; set; }
        public int OtherSeasoning { get; set; }

        /////////BEBIDAS
        public int Soda { get; set; }
        public int LightSoda { get; set; }
        public int Juice { get; set; }
        public int LightJuice { get; set; }
        public int PureWater { get; set; }
        public int Infusion { get; set; }
        public int Te { get; set; }
        public int Coffee { get; set; }

        ///OTHERS
        public int QuantityFood { get; set; }
        public bool IfCollation { get; set; }
        public int NumberCollation { get; set; }
        public string DescriptionCollation { get; set; }
    }
}
