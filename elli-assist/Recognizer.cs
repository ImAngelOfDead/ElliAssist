using Vosk;

public class Recognizer
{//
    private readonly VoskRecognizer _recognizer;

    public Recognizer()
    {
        Model model = new Model("model");
        _recognizer = new VoskRecognizer(model, 16000.0f);
    }

    public string ProcessAudio(byte[] buffer, int bytesRecorded)
    {
        if (_recognizer.AcceptWaveform(buffer, bytesRecorded))
        {
            string resultJson = _recognizer.Result();
            return ExtractText(resultJson);
        }
        return null;
    }

    private string ExtractText(string resultJson)
    {
        int start = resultJson.IndexOf("\"text\" : \"") + 10;
        if (start < 10) return null;

        int end = resultJson.IndexOf("\"", start);
        if (end == -1) return null;

        return resultJson.Substring(start, end - start).Trim().ToLower();
    }
}