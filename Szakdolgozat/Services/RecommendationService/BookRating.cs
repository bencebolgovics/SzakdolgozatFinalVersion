using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdolgozat.Services.RecommendationService
{
    public class BookRating
    {
        [LoadColumn(0)]
        public float userId;
        [LoadColumn(1)]
        public float bookId;
        [LoadColumn(2)]
        public float Label;
    }
}
