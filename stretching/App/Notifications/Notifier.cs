using System.Collections.Generic;
using System.Windows;

namespace Stretching.App.Notifications
{
    class Notifier
    {
        /**
         * Methods Notifies user with text box
         * @param message
         * @param title
         * @param button
         * @param image
         * @return MessageBoxResult
         */
        public MessageBoxResult Notify(MESSAGES message, TITLES title, MessageBoxButton button, MessageBoxImage image) => MessageBox.Show(MESSAGES_DICTIONARY[message], TITLES_DICTIONARY[title], button, image);

        // Enums to map text
        public enum MESSAGES { WRONG_FILE, NO_DATA_IN_FILE, WRONG_DATA, L0_NAN, NO_L0 };
        public enum TITLES { ERROR, WARNING, CONFIRMATION, FILE_ERROR };

        //Dictionaries to get correct text for user
        private readonly static Dictionary<MESSAGES, string> MESSAGES_DICTIONARY = new Dictionary<MESSAGES, string>
        {
            [MESSAGES.WRONG_FILE] = "Wybrano zły typ pliku",
            [MESSAGES.NO_DATA_IN_FILE] = "W wybranym pliku brakuje danych",
            [MESSAGES.WRONG_DATA] = "W wybranym pliku znajdują się niewłaściwe dane",
            [MESSAGES.L0_NAN] = "Wymiar L0 nie jest liczbą",
            [MESSAGES.NO_L0] = "Brak wymiaru L0",
        };

        private readonly static Dictionary<TITLES, string> TITLES_DICTIONARY = new Dictionary<TITLES, string>
        {
            [TITLES.ERROR] = "Bład",
            [TITLES.FILE_ERROR] = "Bład pliku",
            [TITLES.WARNING] = "Uwaga",
            [TITLES.CONFIRMATION] = "Potwierdź",
        };
    }
}
