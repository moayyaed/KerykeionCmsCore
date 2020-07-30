using KerykeionCmsCore.Classes;
using KerykeionCmsCore.Constants;
using KerykeionCmsCore.Dtos;
using KerykeionCmsCore.Enums;
using KerykeionCmsCore.Options;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KerykeionCmsCore.Services
{
    /// <summary>
    /// Provides the API's for translation and delivering CRUD operations on in database stored translations.
    /// </summary>
    public class KerykeionTranslationsService
    {
        public KerykeionCmsOptions Options { get; set; }
        private const string BaseUri = "https://localhost:44370/api/translations";

        /// <summary>
        /// Constructs a new instance of KerykeionCms.Services.KerykeionTranslationsService
        /// </summary>
        /// <param name="options">The KerykeionCms configured options.</param>
        public KerykeionTranslationsService(IOptions<KerykeionCmsOptions> options)
        {
            Options = options.Value;
        }

        /// <summary>
        /// Translates a given piece of text or sentence.
        /// </summary>
        /// <param name="text">The text to be translated.</param>
        /// <returns>
        /// A System.Threading.Tasks.Task that represents the result of the asynchronous query, containing the translation of the specified text.
        /// </returns>
        public async Task<string> TranslateAsync(string text)
        {
            return await CallApiAsync($"translate/{Options.Pages.Language}/{text}");
        }

        /// <summary>
        /// Gets the translation route by the specified text.
        /// </summary>
        /// <param name="text">The text to find the kerykeion translation by.</param>
        /// <returns>
        /// A System.Threading.Tasks.Task that represents the result of the asynchronous query, containing the RouteValueDictionary of the KerykeionTranslation obtained by the specified text.
        /// </returns>
        public async Task<RouteValueDictionary> GetRouteByTextAsync(string text)
        {
            return JsonConvert.DeserializeObject<RouteValueDictionary>(await CallApiAsync($"route/{text}"));
        }

        /// <summary>
        /// Gets a list of languages which are currently not chosen.
        /// </summary>
        /// <param name="chosenLanguage">The current chosen language.</param>
        /// <returns>
        /// A list of languages to choose from except the one that's already chosen.
        /// </returns>
        public List<PickedLanguageDto> NotChosenCmsLanguages(KerykeionCmsLanguage chosenLanguage)
        {
            return chosenLanguage switch
            {
                KerykeionCmsLanguage.NL => new List<PickedLanguageDto>
                    {
                        new PickedLanguageDto
                        {
                            ShortLanguage = "EN",
                            LongLanguage = "English"
                        },
                        new PickedLanguageDto
                        {
                            ShortLanguage = "FR",
                            LongLanguage = "Français"
                        },
                        new PickedLanguageDto
                        {
                            ShortLanguage = "DE",
                            LongLanguage = "Deutsch"
                        }
                    },
                KerykeionCmsLanguage.EN => new List<PickedLanguageDto>
                    {
                        new PickedLanguageDto
                        {
                            ShortLanguage = "NL",
                            LongLanguage = "Nederlands"
                        },
                        new PickedLanguageDto
                        {
                            ShortLanguage = "FR",
                            LongLanguage = "Français"
                        },
                        new PickedLanguageDto
                        {
                            ShortLanguage = "DE",
                            LongLanguage = "Deutsch"
                        }
                    },
                KerykeionCmsLanguage.DE => new List<PickedLanguageDto>
                    {
                        new PickedLanguageDto
                        {
                            ShortLanguage = "EN",
                            LongLanguage = "English"
                        },
                        new PickedLanguageDto
                        {
                            ShortLanguage = "FR",
                            LongLanguage = "Français"
                        },
                        new PickedLanguageDto
                        {
                            ShortLanguage = "NL",
                            LongLanguage = "Nederlands"
                        }
                    },
                KerykeionCmsLanguage.FR => new List<PickedLanguageDto>
                    {
                        new PickedLanguageDto
                        {
                            ShortLanguage = "EN",
                            LongLanguage = "English"
                        },
                        new PickedLanguageDto
                        {
                            ShortLanguage = "NL",
                            LongLanguage = "Nederlands"
                        },
                        new PickedLanguageDto
                        {
                            ShortLanguage = "DE",
                            LongLanguage = "Deutsch"
                        }
                    },
                _ => new List<PickedLanguageDto>
                    {
                        new PickedLanguageDto
                        {
                            ShortLanguage = "NL",
                            LongLanguage = "Nederlands"
                        },
                        new PickedLanguageDto
                        {
                            ShortLanguage = "FR",
                            LongLanguage = "Français"
                        },
                        new PickedLanguageDto
                        {
                            ShortLanguage = "DE",
                            LongLanguage = "Deutsch"
                        }
                    },
            };
        }

        #region ErrorMsgsFunx
        public string TranslateRequiredError(string field)
        {
            return Options.Pages.Language switch
            {
                KerykeionCmsLanguage.NL => $"Het veld '{field}' is verplicht.",
                KerykeionCmsLanguage.EN => $"The field '{field}' is required.",
                KerykeionCmsLanguage.DE => $"Das Feld '{field}' ist erforderlich.",
                KerykeionCmsLanguage.FR => $"Le domaine '{field}' est nécessaire.",
                _ => $"The field '{field}' is required.",
            };
        }

        public string TranslateStringLengthError(int minLength, int maxLength, string field)
        {
            return Options.Pages.Language switch
            {
                KerykeionCmsLanguage.NL => $"Het veld '{field}' moet minstens {minLength} en maximum {maxLength} karakters bevatten.",
                KerykeionCmsLanguage.EN => $"The field '{field}' must contain a minimum of {minLength} and a maximum of {maxLength} characters.",
                KerykeionCmsLanguage.DE => $"Das Feld '{field}' muss mindestens {minLength} und höchstens {maxLength} Zeichen enthalten.",
                KerykeionCmsLanguage.FR => $"Le domaine '{field}' doit contenir un minimum de {minLength} et un maximum de {maxLength} caractères.",
                _ => $"The field '{field}' must contain a minimum of {minLength} and a maximum of {maxLength} characters.",
            };
        }

        public string TranslateCompareValidationError(string field, string fieldToCompare)
        {
            return Options.Pages.Language switch
            {
                KerykeionCmsLanguage.NL => $"Het veld '{field}' en het veld '{fieldToCompare}' komen niet overeen.",
                KerykeionCmsLanguage.EN => $"The field '{field}' and the field '{fieldToCompare}' do not match.",
                KerykeionCmsLanguage.DE => $"Das Feld '{field}' und Das Feld '{fieldToCompare}' stimmen nicht überein.",
                KerykeionCmsLanguage.FR => $"Le domaine '{field}' et le domaine '{fieldToCompare}' ne correspondent pas.",
                _ => $"The field '{field}' and the field '{fieldToCompare}' do not match.",
            };
        }

        /// <summary>
        /// Translates an error specified by its ErrorDescriber to the language set by the KerykeionCmsOptions.
        /// </summary>
        /// <param name="errorDescriber">The ErrorDescriber, to identify the error in the database.</param>
        /// <param name="fallbackMessage">A message to use in case the error is not found in the database.</param>
        /// <param name="newValue">A variable value to use as replacement of the default 'VARIABLEVALUE' in the database.</param>
        /// <returns>
        /// The error text in the language set by the KerykeionCmsOptions
        /// </returns>
        public string TranslateError(string errorDescriber, string fallbackMessage, string newValue)
        {
            return CallApiAsync($"Translate/Error/{Options.Pages.Language}/{errorDescriber}/{newValue}").Result ?? fallbackMessage;
        }

        /// <summary>
        /// Translates an error specified by its HResult to the language set by the KerykeionCmsOptions.
        /// </summary>
        /// <param name="hResult">The HResult identifier, to identify the error in the database.</param>
        /// <param name="fallbackMessage">A message to use in case the error is not found in the database.</param>
        /// <returns>
        /// The error text in the language set by the KerykeionCmsOptions
        /// </returns>
        public string TranslateError(int hResult, string fallbackMessage)
        {
            return CallApiAsync($"Translate/Exception/{Options.Pages.Language}/{hResult}").Result ?? fallbackMessage;
        }


        /// <summary>
        /// Translates an error specified by its ErrorDescriber to the language set by the KerykeionCmsOptions.
        /// </summary>
        /// <param name="errorDescriber">The ErrorDescriber, to identify the error in the database.</param>
        /// <param name="fallbackMessage">A message to use in case the error is not found in the database.</param>
        /// <returns>
        /// The error text in the language set by the KerykeionCmsOptions
        /// </returns>
        public string TranslateError(string errorDescriber, string fallbackMessage)
        {
            return CallApiAsync($"Translate/Error/{Options.Pages.Language}/{errorDescriber}").Result ?? fallbackMessage;
        }
        #endregion

        private async Task<string> CallApiAsync(string requestUri = "")
        {
            using HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync($"{BaseUri}/{requestUri}");
            return await response.Content.ReadAsStringAsync();
        }
    }
}
