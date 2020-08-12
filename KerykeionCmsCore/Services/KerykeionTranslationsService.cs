using KerykeionCmsCore.Dtos;
using KerykeionCmsCore.Enums;
using KerykeionCmsCore.Options;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace KerykeionCmsCore.Services
{
    /// <summary>
    /// Provides the API's for translation and delivering CRUD operations on in database stored translations.
    /// </summary>
    public class KerykeionTranslationsService
    {
        /// <summary>
        /// The KerykeionCms options.
        /// </summary>
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
        /// Translates a given piece of text or sentence, as an asynchronous operation.
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
        
        /// <summary>
        /// Translates an error specified by its ErrorDescriber to the language set by the KerykeionCmsOptions.
        /// </summary>
        /// <param name="errorDescriber">The ErrorDescriber, to identify the error in the database.</param>
        /// <param name="fallbackMessage">A message to use in case the error is not found in the database.</param>
        /// <returns>
        /// The error text in the language set by the KerykeionCmsOptions
        /// </returns>
        public string TranslateErrorByDescriber(string errorDescriber, string fallbackMessage)
        {
            return CallApiAsync($"Translate/Error/{Options.Pages.Language}/{errorDescriber}")?.Result ?? fallbackMessage;
        }

        /// <summary>
        /// Translates an error specified by its ErrorDescriber to the language set by the KerykeionCmsOptions.
        /// </summary>
        /// <param name="errorDescriber">The ErrorDescriber, to identify the error in the database.</param>
        /// <param name="fallbackMessage">A message to use in case the error is not found in the database.</param>
        /// <param name="newValue">A variable value to use as replacement for the default 'FIRSTVARIABLEVALUE' in the database.</param>
        /// <returns>
        /// The error text in the language set by the KerykeionCmsOptions
        /// </returns>
        public string TranslateErrorByDescriber(string errorDescriber, string fallbackMessage, string newValue)
        {
            return CallApiAsync($"Translate/Error/{Options.Pages.Language}/{errorDescriber}/{newValue}")?.Result ?? fallbackMessage;
        }

        /// <summary>
        /// Translates an error specified by its ErrorDescriber to the language set by the KerykeionCmsOptions.
        /// </summary>
        /// <param name="errorDescriber">The ErrorDescriber, to identify the error in the database.</param>
        /// <param name="fallbackMessage">A message to use in case the error is not found in the database.</param>
        /// <param name="newValue">A variable value to use as replacement for the default 'FIRSTVARIABLEVALUE' in the database.</param>
        /// <param name="secondNewValue">A variable value to use as replacement for the default 'SECONDVARIABLEVALUE' in the database.</param>
        /// <returns>
        /// The error text in the language set by the KerykeionCmsOptions
        /// </returns>
        public string TranslateErrorByDescriber(string errorDescriber, string fallbackMessage, string newValue, string secondNewValue)
        {
            return CallApiAsync($"Translate/Error/{Options.Pages.Language}/{errorDescriber}/{newValue}/{secondNewValue}")?.Result ?? fallbackMessage;
        }

        /// <summary>
        /// Translates an error specified by its ErrorDescriber to the language set by the KerykeionCmsOptions.
        /// </summary>
        /// <param name="errorDescriber">The ErrorDescriber, to identify the error in the database.</param>
        /// <param name="fallbackMessage">A message to use in case the error is not found in the database.</param>
        /// <param name="newValue">A variable value to use as replacement for the default 'FIRSTVARIABLEVALUE' in the database.</param>
        /// <param name="secondNewValue">A variable value to use as replacement for the default 'SECONDVARIABLEVALUE' in the database.</param>
        /// <param name="thirdNewValue">A variable value to use as replacement for the default 'THIRDVARIABLEVALUE' in the database.</param>
        /// <returns>
        /// The error text in the language set by the KerykeionCmsOptions
        /// </returns>
        public string TranslateErrorByDescriber(string errorDescriber, string fallbackMessage, string newValue, string secondNewValue, string thirdNewValue)
        {
            return CallApiAsync($"Translate/Error/{Options.Pages.Language}/{errorDescriber}/{newValue}/{secondNewValue}/{thirdNewValue}")?.Result ?? fallbackMessage;
        }

        /// <summary>
        /// Searches for the translation of the KerykeionCms documentation, as an asynchronous operation.
        /// </summary>
        /// <param name="id">The id of the document translation to search for.</param>
        /// <returns>
        /// A System.Threading.Tasks.Task that represents the result of the asynchronous query, containing the string of the KerykeionTranslation which matches the specified ID.
        /// </returns>
        public async Task<string> FindDocByIdAsync(Guid id)
        {
            return await CallApiAsync($"Documentation/{Options.Pages.Language}/{id}");
        }

        private async Task<string> CallApiAsync(string requestUri = "")
        {
            using HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync($"{BaseUri}/{requestUri}");
            return await response.Content.ReadAsStringAsync();
        }
    }
}
