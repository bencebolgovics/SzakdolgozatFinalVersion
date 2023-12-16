using DevExpress.Mvvm.Native;
using Microsoft.ML;
using Microsoft.ML.Trainers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Szakdolgozat.Services.RecommendationService
{
    public static class BookRecommendation
    {
        public static string trainingDataPath = "C:\\Users\\Bence\\Desktop\\Szakdolgozat\\Szakdolgozat\\Szakdolgozat\\Services\\RecommendationService\\DataForModel\\bookRatingTrain.csv";
        private static string testDataPath = "C:\\Users\\Bence\\Desktop\\Szakdolgozat\\Szakdolgozat\\Szakdolgozat\\Services\\RecommendationService\\DataForModel\\bookRatingsTest.csv";
        private static string modelPath = "C:\\Users\\Bence\\Desktop\\Szakdolgozat\\Szakdolgozat\\Szakdolgozat\\Services\\RecommendationService\\DataForModel\\BookRecommenderModel.zip";

        //NEED FOR TRAINING
        public static (IDataView training, IDataView test) LoadData(MLContext mlContext)
        {
            IDataView trainingDataView = mlContext.Data.LoadFromTextFile<BookRating>(trainingDataPath, hasHeader: true, separatorChar: ';');
            IDataView testDataView = mlContext.Data.LoadFromTextFile<BookRating>(testDataPath, hasHeader: true, separatorChar: ';');

            return (trainingDataView, testDataView);
        }

        //NEED FOR TRAINING
        public static ITransformer BuildAndTrainModel(MLContext mlContext, IDataView trainingDataView)
        {
            IEstimator<ITransformer> estimator = mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "userIdEncoded", inputColumnName: "userId")
                .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "bookIdEncoded", inputColumnName: "bookId"));

            var options = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = "userIdEncoded",
                MatrixRowIndexColumnName = "bookIdEncoded",
                LabelColumnName = "Label",
                NumberOfIterations = 20,
                ApproximationRank = 100
            };

            var trainerEstimator = estimator.Append(mlContext.Recommendation().Trainers.MatrixFactorization(options));

            ITransformer model = trainerEstimator.Fit(trainingDataView);

            return model;
        }

        public static List<float> UseModelForRanking(MLContext mlContext, ITransformer model, List<int> bookIds)
        {
            var predictionEngine = mlContext.Model.CreatePredictionEngine<BookRating, BookRatingPrediction>(model);

            List<(float, float)> predictions = new();

            foreach (var id in bookIds)
            {
                var testInput = new BookRating { userId = 1, bookId = id };

                var bookRatingPrediction = predictionEngine.Predict(testInput);

                predictions.Add((id, bookRatingPrediction.Score));
            }

            return predictions.OrderBy(x => x.Item2).Take(3).Select(x => x.Item1).ToList();
        }

        //NEED FOR TRAINING
        public static void SaveModel(MLContext mlContext, DataViewSchema trainingDataViewSchema, ITransformer model)
        {
            var modelPath = "C:\\Users\\Bence\\Desktop\\Szakdolgozat\\Szakdolgozat\\Szakdolgozat\\Services\\RecommendationService\\DataForModel\\BookRecommenderModel.zip";

            mlContext.Model.Save(model, trainingDataViewSchema, modelPath);
        }

        public static ITransformer LoadModel(MLContext context)
        {
            DataViewSchema modelSchema;

            return context.Model.Load(modelPath, out modelSchema);
        }
    }
}
