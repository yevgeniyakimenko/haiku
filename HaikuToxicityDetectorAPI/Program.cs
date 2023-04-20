using System.Linq;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/v1/classify", (string text) =>
{
    ToxicModel.ModelInput input = new ToxicModel.ModelInput { Text = text };

    var sortedScoresWithLabel = ToxicModel.PredictAllLabels(input);
    var isToxic = sortedScoresWithLabel.First().Key == "1";

    return new
    {
        isToxic,
        scores = sortedScoresWithLabel
    };
});

app.Run();
