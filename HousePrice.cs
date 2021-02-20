using Microsoft.ML.Data;

namespace HousePricePrediction
{
    public class ToAreaInThousands
    {
        public float LotAreaThousands { get; set; }
        public float TotalBasementSquareFeetThousands { get; set; }
        public float GroundLivingAreaThousands { get; set; }
        public float GarageAreaThousands { get; set; }
    }

    public class HousePricePrediction
    {
        [ColumnName("Score")]
        public float HousePrice { get; set; }
    }

    public class HousePrice
    {
        //[LoadColumn(0)]
        //public string Id { get; set; }
        [LoadColumn(1)]
        public string MSSubClass { get; set; }
        //[LoadColumn(2)]
        //public string MSZoning { get; set; }
        [LoadColumn(3)] // Can be NA but usually a number
        public float LotFrontage { get; set; }
        [LoadColumn(4)]
        public float LotArea { get; set; }
        //[LoadColumn(5)]
        //public string Street { get; set; }
        //[LoadColumn(6)]
        //public string Alley { get; set;}
        //[LoadColumn(7)]
        //public string LotShape { get; set; }
        //[LoadColumn(8)]
        //public string LandContour { get; set; }
        //[LoadColumn(9)]
        //public string Utilities { get; set;}
        //[LoadColumn(10)]
        //public string LotConfig { get; set;}
        //[LoadColumn(11)]
        //public string LandSlope { get; set;}
        [LoadColumn(12)]
        public string Neighbourhood { get; set; }
        //[LoadColumn(13)]
        //public string Condition1 { get; set; }
        //[LoadColumn(14)]
        //public string Condition2 { get; set; }
        [LoadColumn(15)]
        public string BuildingType { get; set; }
        [LoadColumn(16)]
        public string HouseStyle { get; set; }
        [LoadColumn(17)]
        public float OverallQuality { get; set; }
        [LoadColumn(18)]
        public float OverallCondition { get; set; }
        [LoadColumn(19)]
        public float YearBuilt { get; set; }
        [LoadColumn(20)]
        public float YearRemodAdd { get; set; }
        //[LoadColumn(21)]
        //public string RoofStyle { get; set; }
        //[LoadColumn(22)]
        //public string RoofMatl { get; set; }
        [LoadColumn(23)]
        public string Exterior1st { get; set; }
        [LoadColumn(24)]
        public string Exterior2nd { get; set; }
        //[LoadColumn(25)]
        //public string MasVnrType { get; set; }
        //[LoadColumn(26)]
        //public int MasVnrArea { get; set; }
        //[LoadColumn(27)]
        //public string ExterQual { get; set; }
        //[LoadColumn(28)]
        //public string ExterCond { get; set; }
        [LoadColumn(29)]
        public string Foundation { get; set; }
        //[LoadColumn(30)]
        //public string BsmtQual { get; set; }
        //[LoadColumn(31)]
        //public string BsmtCond { get; set; }
        //[LoadColumn(32)]
        //public string BsmtExposure { get; set; }
        //[LoadColumn(33)]
        //public string BsmtFinType1 { get; set; }
        //[LoadColumn(34)]
        //public int BsmtFinSF1 { get; set; }
        //[LoadColumn(35)]
        //public string BsmtFinType2 { get; set; }
        //[LoadColumn(36)]
        //public int BsmtFinSF2 { get; set; }
        //[LoadColumn(37)]
        //public int BsmtUnfSF { get; set; }
        [LoadColumn(38)]
        public float TotalBasementSquareFeet { get; set; }
        //[LoadColumn(39)]
        //public string Heating { get; set; }
        //[LoadColumn(40)]
        //public string HeatingQC { get; set; }
        //[LoadColumn(41)]
        //public string CentralAir { get; set; }
        //[LoadColumn(42)]
        //public string Electrical { get; set; }
        //[LoadColumn(43)]
        //public int FirstFlrSF { get; set; }
        //[LoadColumn(44)]
        //public int SecondFlrSF { get; set; }
        //[LoadColumn(45)]
        //public int LowQualFinSF { get; set; }
        [LoadColumn(46)]
        public float GroundLivingArea { get; set; }
        //[LoadColumn(47)]
        //public int BsmtFullBath { get; set; }
        //[LoadColumn(48)]
        //public int BsmtHalfBath { get; set; }
        [LoadColumn(49)]
        public float FullBath { get; set; }
        //[LoadColumn(50)]
        //public int HalfBath { get; set; }
        [LoadColumn(51)]
        public float BedroomsAboveGround { get; set; }
        //[LoadColumn(52)]
        //public int KitchenAbvGr { get; set; }
        [LoadColumn(53)]
        public string KitchenQuality { get; set; }
        [LoadColumn(54)]
        public float TotalRoomsAboveGround { get; set; }
        //[LoadColumn(55)]
        //public string Functional { get; set; }
        //[LoadColumn(56)]
        //public int Fireplaces { get; set; }
        //[LoadColumn(57)]
        //public string FireplaceQu { get; set; }
        [LoadColumn(58)]
        public string GarageType { get; set; }
        //[LoadColumn(59)]
        //public string GarageYrBlt { get; set; }
        [LoadColumn(60)]
        public string GarageFinish { get; set; }
        [LoadColumn(61)]
        public float GarageCars { get; set; }
        [LoadColumn(62)]
        public float GarageArea { get; set; }
        //[LoadColumn(63)]
        //public string GarageQual { get; set; }
        //[LoadColumn(64)]
        //public string GarageCond { get; set; }
        //[LoadColumn(65)]
        //public string PavedDrive { get; set; }
        //[LoadColumn(66)]
        //public int WoodDeckSF { get; set; }
        //[LoadColumn(67)]
        //public int OpenPorchSF { get; set; }
        //[LoadColumn(68)]
        //public int EnclosedPorch { get; set; }
        //[LoadColumn(69)]
        //public int ThreeSsnPorch { get; set; }
        //[LoadColumn(70)]
        //public int ScreenPorch { get; set; }
        //[LoadColumn(71)]
        //public int PoolArea { get; set; }
        //[LoadColumn(72)]
        //public string PoolQC { get; set; }
        //[LoadColumn(73)]
        //public string Fence { get; set; }
        //[LoadColumn(74)]
        //public string MiscFeature { get; set; }
        //[LoadColumn(75)]
        //public int MiscVal { get; set; }
        //[LoadColumn(76)]
        //public int MoSold { get; set; }
        //[LoadColumn(77)]
        //public string YrSold { get; set; }
        //[LoadColumn(78)]
        //public string SaleType { get; set; }
        //[LoadColumn(79)]
        //public string SaleCondition { get; set; }
        [LoadColumn(80)]
        public float SalePrice { get; set; }
    }
}