namespace Translator.Infrastructure
{
    // Интерфейс для копирования текста text в буфер обмена (реализуется на каждой платформе отдельно)
    public interface ICopyToClipboard   
    {
        void Copy(string text);
    }
}
