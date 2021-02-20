using System;
using System.IO;
using System.Linq;
using BetterConsoleTables;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers.FastTree;

namespace HousePricePrediction
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new MLContext();

            var data = LoadData(context);

            var pipeline = BuildPipeline(context);

            //Preview(pipeline, data);

            var model = TrainModel(context, data, pipeline);

            PredictPrices(context, model);

            Console.WriteLine("Press enter to finish");
            Console.Read();
        }

        private static void Preview(
            IEstimator<ITransformer> pipeline,
            IDataView data)
        {
            var model = pipeline.Fit(data);
            var transformedData = model.Transform(data);
            var preview = transformedData.Preview(maxRows: 10);
            WritePreview(preview);
        }

        public static void WritePreview(DataDebuggerPreview preview)
        {
            // set up a console table
            var table = new Table(
                TableConfiguration.Unicode(),
                (from c in preview.ColumnView 
                    select new ColumnHeader(c.Column.Name)).ToArray());

            // fill the table with results
            foreach (var row in preview.RowView)
            {
                table.AddRow((from c in row.Values 
                        select c.Value is VBuffer<float> ? "<vector>" : c.Value
                    ).ToArray());
            }

            // write the table
            Console.WriteLine(table.ToString());
        }

        private static IDataView LoadData(
            MLContext context)
        {
            Console.Write("Loading data...");

            var dataPath = Path.Combine(Environment.CurrentDirectory, "data.csv");
            var textLoader = context.Data.CreateTextLoader(
                new TextLoader.Options
                {
                    Separators = new[] {','},
                    HasHeader = true,
                    Columns = new[]
                    {
                        new TextLoader.Column("SalePrice", DataKind.Single, 80),
                        new TextLoader.Column("LotArea", DataKind.Single, 4),
                        new TextLoader.Column("OverallQuality", DataKind.Single, 17),
                        new TextLoader.Column("OverallCondition", DataKind.Single, 18),
                        new TextLoader.Column("YearBuilt", DataKind.Single, 19),
                        new TextLoader.Column("YearRemodAdd", DataKind.Single, 20),
                        new TextLoader.Column("YearSold", DataKind.Single, 77),
                        new TextLoader.Column("MSSubClass", DataKind.String, 1),
                        new TextLoader.Column("Neighbourhood", DataKind.String, 12),
                        new TextLoader.Column("TotalBasementSquareFeet", DataKind.Single, 38),
                        new TextLoader.Column("GroundLivingArea", DataKind.Single, 46),
                        new TextLoader.Column("FullBath", DataKind.Single, 49),
                        new TextLoader.Column("TotalRoomsAboveGround", DataKind.Single, 54),
                        new TextLoader.Column("BedroomsAboveGround", DataKind.Single, 51),
                        new TextLoader.Column("GarageCars", DataKind.Single, 61),
                        new TextLoader.Column("BuildingType", DataKind.String, 15),
                        new TextLoader.Column("HouseStyle", DataKind.String, 16),
                        new TextLoader.Column("KitchenQuality", DataKind.String, 53),
                        new TextLoader.Column("GarageArea", DataKind.Single,62),
                        new TextLoader.Column("Foundation", DataKind.String, 29),
                        new TextLoader.Column("Exterior1st", DataKind.String, 23),
                        new TextLoader.Column("Exterior2nd", DataKind.String, 24),
                        new TextLoader.Column("LotFrontage", DataKind.Single, 3),
                        new TextLoader.Column("GarageFinish", DataKind.String, 60),
                        new TextLoader.Column("GarageType", DataKind.String, 58)
                    }
                });

            var data = textLoader.Load(dataPath);

            Console.WriteLine("done!");

            return data;
        }

        private static IEstimator<ITransformer> BuildPipeline(
            MLContext context)
        {
            Console.Write("Building pipeline...");

            var pipeline =
                context.Transforms
                    .CopyColumns(inputColumnName: "SalePrice", outputColumnName: "Label")
                    .Append(context.Transforms.CustomMapping<HousePrice, ToAreaInThousands>((
                        input,
                        output) =>
                    {
                        output.LotAreaThousands = input.LotArea / 1000;
                        output.GroundLivingAreaThousands = input.GroundLivingArea / 1000;
                        output.TotalBasementSquareFeetThousands = input.TotalBasementSquareFeet / 1000;
                        output.GarageAreaThousands = input.GarageArea / 1000;
                    }, contractName: "AreaInThousands"))
                    //.Append(context.Transforms.Categorical.OneHotEncoding("MSSubClass"))
                    .Append(context.Transforms.Categorical.OneHotEncoding("Neighbourhood"))
                    .Append(context.Transforms.Categorical.OneHotEncoding("BuildingType"))
                    .Append(context.Transforms.Categorical.OneHotEncoding("HouseStyle"))
                    .Append(context.Transforms.Categorical.OneHotEncoding("KitchenQuality"))
                    .Append(context.Transforms.Categorical.OneHotEncoding("Foundation"))
                    .Append(context.Transforms.Categorical.OneHotEncoding("Exterior1st"))
                    .Append(context.Transforms.Categorical.OneHotEncoding("Exterior2nd"))
                    //.Append(context.Transforms.Categorical.OneHotEncoding("YearSold"))
                    .Append(context.Transforms.Categorical.OneHotEncoding("GarageFinish"))
                    .Append(context.Transforms.Categorical.OneHotEncoding("GarageType"))
                    //.Append(context.Transforms.Categorical.OneHotEncoding("FullBath"))
                    .Append(context.Transforms.Concatenate(
                        "Features",
                        "LotAreaThousands",
                        "OverallQuality",
                        "OverallCondition",
                        "YearBuilt",
                        //"YearRemodAdd",
                        ////"YearSold",
                        //////"MSSubClass",
                        "Neighbourhood",
                        "TotalBasementSquareFeetThousands",
                        "GroundLivingAreaThousands",
                        "FullBath",
                        //"TotalRoomsAboveGround",
                        "BedroomsAboveGround",
                        "GarageCars",
                        "BuildingType",
                        "HouseStyle",
                        //"GarageType",
                        //"KitchenQuality"
                        //"GarageAreaThousands",
                        //"GarageFinish",
                        "Foundation"
                        //"Exterior1st"
                        //"Exterior2nd",
                        //"LotFrontage"
                        ))
                    .AppendCacheCheckpoint(context)
                    .Append(context.Regression.Trainers.FastTree());

            Console.WriteLine("done!");

            return pipeline;
        }

        private static ITransformer TrainModel(
            MLContext context,
            IDataView data,
            IEstimator<ITransformer> pipeline)
        {
            Console.Write("Training the model...");
            var partitions = context.Data.TrainTestSplit(data, testFraction: 0.2);
            var model = pipeline.Fit(partitions.TrainSet);
            var predictions = model.Transform(partitions.TestSet);

            var metrics = context.Regression.Evaluate(predictions, "Label", "Score");

            Console.WriteLine("done!");

            Console.WriteLine();
            Console.WriteLine($"Model metrics:");
            Console.WriteLine($"  RMSE:{metrics.RootMeanSquaredError:#.##}");
            Console.WriteLine($"  MSE: {metrics.MeanSquaredError:#.##}");
            Console.WriteLine($"  MAE: {metrics.MeanAbsoluteError:#.##}");
            Console.WriteLine();

            return model;
        }

        private static void PredictPrices(
            MLContext context,
            ITransformer model)
        {
            var predictionFunction = 
                context.Model.CreatePredictionEngine<HousePrice, HousePricePrediction>(model);


            while (true)
            {
                Console.WriteLine("Iowa House Price Prediction");
                var housePrice = new HousePrice();
                Console.Write("Enter Lot Area: ");
                housePrice.LotArea = Convert.ToSingle(Console.ReadLine());
                Console.Write("Enter Overall Quality: ");
                housePrice.OverallQuality = Convert.ToSingle(Console.ReadLine());
                Console.Write("Enter Overall Condition: ");
                housePrice.OverallCondition = Convert.ToSingle(Console.ReadLine());
                Console.Write("Enter Year Built: ");
                housePrice.YearBuilt = Convert.ToSingle(Console.ReadLine());
                Console.Write("Enter Neighbourhood: ");
                housePrice.Neighbourhood = Console.ReadLine();
                Console.Write("Enter Basement Square Feet: ");
                housePrice.TotalBasementSquareFeet = Convert.ToSingle(Console.ReadLine());
                Console.Write("Enter Ground Living Area: ");
                housePrice.GroundLivingArea = Convert.ToSingle(Console.ReadLine());
                Console.Write("Enter Full Bathrooms: ");
                housePrice.FullBath = Convert.ToSingle(Console.ReadLine());
                Console.Write("Enter Bedrooms Above Ground: ");
                housePrice.BedroomsAboveGround = Convert.ToSingle(Console.ReadLine());
                Console.Write("Enter Garage Cars: ");
                housePrice.GarageCars = Convert.ToSingle(Console.ReadLine());
                Console.Write("Enter Building Type: ");
                housePrice.BuildingType = Console.ReadLine();
                Console.Write("Enter House Style: ");
                housePrice.HouseStyle = Console.ReadLine();
                Console.Write("Enter Foundation: ");
                housePrice.Foundation = Console.ReadLine();

                var prediction = predictionFunction.Predict(housePrice);

                Console.WriteLine($"House price prediction: {prediction.HousePrice}");

                Console.WriteLine("Predict again?");
                if (Console.ReadLine() != "Y")
                {
                    break;
                }
            }
        }
    }
}
