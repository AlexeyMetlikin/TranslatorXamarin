using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using Translator.Model;
using Xamarin.Forms;
using Translator.Infrastructure;
using System.Threading.Tasks;
using Plugin.Settings;

namespace Translator
{
    public partial class MainPage : ContentPage
    {
        private TranslatorApi _API;

        public MainPage(TranslatorApi API)
        {
            InitializeComponent();

            _API = API;
            GetLanguagesAsync();
        }

        private async void GetLanguagesAsync()
        {
            var pars = new List<KeyValuePair<string, string>>                       // ��������� ��������� ��� ���������� �������
                {
                    new KeyValuePair<string, string>( "ui", "ru" ),                 // �� ����� ����� �������� ������
                    new KeyValuePair<string, string>( "key", _API.API_Key )         // ���� API
                };
            string response = await SendPostRequestAsync("/api/v1.5/tr.json/getLangs", pars);  // ��������� ������ � API �� ��������� ������ ������

            if (response != null)   // ���� ����� �������
            {
                try
                {
                    _API.FillLanguages(response);   // ��������� ������ ������ � TranslateAPI
                    FillPickers();                  // ��������� �omboBox'� �� TranslateAPI
                }
                catch (IndexOutOfRangeException exp)
                {
                    await DisplayAlert("������", exp.Message, "��");
                }
                catch (Exception exp)
                {
                    await DisplayAlert("�������������� ������", exp.Message, "��");
                }
            }

        }

        private void FillPickers()
        {
            if (_API.Langs.Count > 0)
            {
                foreach (var lang in _API.Langs.OrderBy(l => l.Value))
                {
                    langFrom.Items.Add(lang.Value);
                    langTo.Items.Add(lang.Value);
                }
                SetDefaultLanguage(langFrom, "����������");
                SetDefaultLanguage(langTo, "�������");
            }
        }

        private void SetDefaultLanguage(Picker picker, string lang)
        {
            if (picker.Items.Contains(lang))
            {
                picker.SelectedItem = picker.Items[picker.Items.IndexOf(lang)];
            }
            else
            {
                picker.SelectedItem = picker.Items[0];
            }
        }

        private void OnTapReplaceLanguage(object sender, EventArgs e)
        {
            if (langFrom.SelectedItem != null && langTo.SelectedItem != null)
            {
                var temp = langFrom.SelectedItem;
                langFrom.SelectedItem = langTo.SelectedItem;
                langTo.SelectedItem = temp;
            }
        }

        private async void Button_TranslateButtonClick(object sender, EventArgs e)
        {
            try
            {
                if (langFrom.SelectedIndex == -1)
                {
                    langFrom.SelectedIndex = langFrom.Items.IndexOf(await TryDetectLangAsync(original.Text));
                }
                if (original.Text == null)
                {
                    throw new NullReferenceException("����������� ����� ��� ��������");
                }
                translaion.Text = await TranslateAsync(original.Text,
                                            _API.Langs.Find(l => l.Value == langFrom.SelectedItem.ToString()).Key,
                                            _API.Langs.Find(l => l.Value == langTo.SelectedItem.ToString()).Key);
            }
            catch (ArgumentNullException exp)
            {
                await DisplayAlert("������", exp.Message, "��");
            }
            catch (NullReferenceException exp)
            {
                await DisplayAlert("������", exp.Message, "��");
            }
            catch (Exception exp)
            {
                await DisplayAlert("�������������� ������", exp.Message, "��");
            }
        }

        private async void Editor_originalTextCompleted(object sender, EventArgs e)
        {
            if (langFrom.SelectedIndex == -1 && !String.IsNullOrEmpty(original.Text))  // ���� ���� �� ������ - ���������� �������������
            {
                langFrom.SelectedIndex = langFrom.Items.IndexOf(await TryDetectLangAsync(original.Text));
            }
        }

        // ������� ��� ������� �� ������ ������� ��������� ������
        private void OnTapClearOriginalText(object sender, EventArgs e)
        {
            original.Text = null;
            translaion.Text = null;
        }

        // ������� ��� ������� �� ������ ����������� ������������� ������
        private void OnTapCopyTranslationText(object sender, EventArgs e)
        {
            CopyToClipboard(translaion.Text);
        }

        private void CopyToClipboard(string text)
        {
            DependencyService.Get<ICopyToClipboard>().Copy(text);
        }

        // ��������� ������� ������ translateFrom � ����� langFrom �� ���� langTo
        public async Task<string> TranslateAsync(string translateFrom, string langFrom, string langTo)
        {
            string lang = langFrom;
            if (langTo == null)
            {
                throw new ArgumentNullException("�� ����� ���� ��������");
            }
            if (lang == null)
            {
                lang = langTo;
            }
            else
            {
                lang += "-" + langTo;
            }

            string translation = "";

            // ���� ����� ������� ��������� 10000 �������� - ������� ������ �� ��������� � ����������  
            for (int i = 0; i < translateFrom.Length; i += 10000)
            {
                var text = translateFrom;
                if (text.Length > 9999)
                {
                    text = text.Substring(i, i + 9999);
                }
                var pars = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>( "lang", lang ),           // �����, � ������ � �� ����� ���������� � ������� 'en-ru'   
                        new KeyValuePair<string, string>( "option", "1" ),          // ����������, ��������� �� ������ ����, � �������� ����� ���������
                        new KeyValuePair<string, string>( "text", text ),           // ����� ��� ��������
                        new KeyValuePair<string, string>( "key", _API.API_Key )     // ����
                    };
                var response = await SendPostRequestAsync("/api/v1.5/tr.json/translate", pars);   // ��������� ������

                if (response != null)
                {
                    translation += new Regex("\"text\":").Split(response)[1].Split('[', ']')[1].Replace("\"", "");  // ���� ������ ������� �������� - �������� ������� � �������������� ������
                }
            }
            return translation;
        }

        // ���������� ���� ��������� ������
        public async Task<string> TryDetectLangAsync(string textForDetect)
        {
            var pars = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>( "hint", _API.Langs.Find(l => l.Value == textForDetect).Key ),  // �������� ��������� ���� (�����) 'en,ru,...'
                    new KeyValuePair<string, string>( "text", textForDetect),       // ����� ��� ����������� �����
                    new KeyValuePair<string, string>( "key", _API.API_Key )         // ����
                };
            string response = await SendPostRequestAsync("/api/v1.5/tr.json/detect", pars);

            if (response != null)
            {
                var langKey = new Regex("\"lang\":").Split(response)[1].Split('}')[0].Replace("\"", "");    // ���� ������ ������� �������� - ������� ���� ������
                return _API.Langs.Find(l => l.Key == langKey).Value;
            }
            return null;
        }

        private async Task<string> SendPostRequestAsync(string request, List<KeyValuePair<string, string>> pars)
        {
            try
            {
                return await _API.SendPostAync(request, pars);
            }
            catch (HttpRequestException exp)
            {
                await DisplayAlert("������ ������� �� ������", exp.Message, "��");
            }
            catch (Exception exp)
            {
                await DisplayAlert("�������������� ������", exp.Message, "��");
            }
            return null;
        }

        private void translaion_Focused(object sender, FocusEventArgs e)
        {

        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 