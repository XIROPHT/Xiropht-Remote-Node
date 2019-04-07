using System;
using System.Collections.Generic;


namespace Xiropht_RemoteNode.Object
{
    public class BigDictionaryTransactionHash // Limited to 1 009 999 999 transactions hash
    {
        private Dictionary<string, long> _bigDictionaryTransaction1; // 0 - 9 999 999
        private Dictionary<string, long> _bigDictionaryTransaction2; // 9 999 999 - 19 999 999
        private Dictionary<string, long> _bigDictionaryTransaction3; // 19 999 999 - 29 999 999
        private Dictionary<string, long> _bigDictionaryTransaction4; // 29 999 999 - 39 999 999
        private Dictionary<string, long> _bigDictionaryTransaction5; // 39 999 999 - 49 999 999
        private Dictionary<string, long> _bigDictionaryTransaction6; // 99 999 999 - 59 999 999
        private Dictionary<string, long> _bigDictionaryTransaction7; // 59 999 999 - 69 999 999
        private Dictionary<string, long> _bigDictionaryTransaction8; // 79 999 999 - 89 999 999
        private Dictionary<string, long> _bigDictionaryTransaction9; // 89 999 999 - 99 999 999 
        private Dictionary<string, long> _bigDictionaryTransaction10; // 99 999 999 - 109 999 999


        private Dictionary<string, long> _bigDictionaryTransaction11; // 109 999 999 - 119 999 999
        private Dictionary<string, long> _bigDictionaryTransaction12; // 119 999 999 - 129 999 999
        private Dictionary<string, long> _bigDictionaryTransaction13; // 129 999 999 - 139 999 999
        private Dictionary<string, long> _bigDictionaryTransaction14; // 139 999 999 - 149 999 999
        private Dictionary<string, long> _bigDictionaryTransaction15; // 149 999 999 - 159 999 999
        private Dictionary<string, long> _bigDictionaryTransaction16; // 159 999 999 - 169 999 999
        private Dictionary<string, long> _bigDictionaryTransaction17; // 169 999 999 - 179 999 999
        private Dictionary<string, long> _bigDictionaryTransaction18; // 179 999 999 - 189 999 999
        private Dictionary<string, long> _bigDictionaryTransaction19; // 189 999 999 - 199 999 999
        private Dictionary<string, long> _bigDictionaryTransaction20; // 199 999 999 - 209 999 999

        private Dictionary<string, long> _bigDictionaryTransaction21; // 209 999 999 - 219 999 999
        private Dictionary<string, long> _bigDictionaryTransaction22; // 219 999 999 - 229 999 999
        private Dictionary<string, long> _bigDictionaryTransaction23; // 229 999 999 - 239 999 999
        private Dictionary<string, long> _bigDictionaryTransaction24; // 239 999 999 - 249 999 999
        private Dictionary<string, long> _bigDictionaryTransaction25; // 249 999 999 - 259 999 999
        private Dictionary<string, long> _bigDictionaryTransaction26; // 259 999 999 - 269 999 999
        private Dictionary<string, long> _bigDictionaryTransaction27; // 269 999 999 - 279 999 999
        private Dictionary<string, long> _bigDictionaryTransaction28; // 279 999 999 - 289 999 999
        private Dictionary<string, long> _bigDictionaryTransaction29; // 289 999 999 - 299 999 999
        private Dictionary<string, long> _bigDictionaryTransaction30; // 299 999 999 - 309 999 999 

        private Dictionary<string, long> _bigDictionaryTransaction31; // 309 999 999 - 319 999 999
        private Dictionary<string, long> _bigDictionaryTransaction32; // 319 999 999 - 329 999 999
        private Dictionary<string, long> _bigDictionaryTransaction33; // 329 999 999 - 339 999 999
        private Dictionary<string, long> _bigDictionaryTransaction34; // 339 999 999 - 349 999 999
        private Dictionary<string, long> _bigDictionaryTransaction35; // 349 999 999 - 359 999 999
        private Dictionary<string, long> _bigDictionaryTransaction36; // 359 999 999 - 369 999 999
        private Dictionary<string, long> _bigDictionaryTransaction37; // 369 999 999 - 379 999 999
        private Dictionary<string, long> _bigDictionaryTransaction38; // 379 999 999 - 389 999 999
        private Dictionary<string, long> _bigDictionaryTransaction39; // 389 999 999 - 399 999 999
        private Dictionary<string, long> _bigDictionaryTransaction40; // 399 999 999 - 409 999 999

        private Dictionary<string, long> _bigDictionaryTransaction41; // 409 999 999 - 419 999 999
        private Dictionary<string, long> _bigDictionaryTransaction42; // 419 999 999 - 429 999 999
        private Dictionary<string, long> _bigDictionaryTransaction43; // 429 999 999 - 439 999 999
        private Dictionary<string, long> _bigDictionaryTransaction44; // 439 999 999 - 449 999 999
        private Dictionary<string, long> _bigDictionaryTransaction45; // 449 999 999 - 459 999 999
        private Dictionary<string, long> _bigDictionaryTransaction46; // 459 999 999 - 469 999 999
        private Dictionary<string, long> _bigDictionaryTransaction47; // 469 999 999 - 479 999 999
        private Dictionary<string, long> _bigDictionaryTransaction48; // 479 999 999 - 489 999 999
        private Dictionary<string, long> _bigDictionaryTransaction49; // 489 999 999 - 499 999 999
        private Dictionary<string, long> _bigDictionaryTransaction50; // 499 999 999 - 509 999 999

        private Dictionary<string, long> _bigDictionaryTransaction51; // 509 999 999 - 519 999 999
        private Dictionary<string, long> _bigDictionaryTransaction52; // 519 999 999 - 529 999 999
        private Dictionary<string, long> _bigDictionaryTransaction53; // 529 999 999 - 539 999 999
        private Dictionary<string, long> _bigDictionaryTransaction54; // 539 999 999 - 549 999 999
        private Dictionary<string, long> _bigDictionaryTransaction55; // 549 999 999 - 559 999 999
        private Dictionary<string, long> _bigDictionaryTransaction56; // 559 999 999 - 569 999 999
        private Dictionary<string, long> _bigDictionaryTransaction57; // 569 999 999 - 579 999 999
        private Dictionary<string, long> _bigDictionaryTransaction58; // 579 999 999 - 589 999 999
        private Dictionary<string, long> _bigDictionaryTransaction59; // 589 999 999 - 599 999 999
        private Dictionary<string, long> _bigDictionaryTransaction60; // 599 999 999 - 609 999 999 

        private Dictionary<string, long> _bigDictionaryTransaction61; // 609 999 999 - 619 999 999
        private Dictionary<string, long> _bigDictionaryTransaction62; // 619 999 999 - 629 999 999
        private Dictionary<string, long> _bigDictionaryTransaction63; // 629 999 999 - 639 999 999
        private Dictionary<string, long> _bigDictionaryTransaction64; // 639 999 999 - 649 999 999
        private Dictionary<string, long> _bigDictionaryTransaction65; // 649 999 999 - 659 999 999
        private Dictionary<string, long> _bigDictionaryTransaction66; // 659 999 999 - 669 999 999
        private Dictionary<string, long> _bigDictionaryTransaction67; // 669 999 999 - 679 999 999
        private Dictionary<string, long> _bigDictionaryTransaction68; // 679 999 999 - 689 999 999
        private Dictionary<string, long> _bigDictionaryTransaction69; // 689 999 999 - 699 999 999
        private Dictionary<string, long> _bigDictionaryTransaction70; // 699 999 999 - 709 999 999 

        private Dictionary<string, long> _bigDictionaryTransaction71; // 709 999 999 - 719 999 999
        private Dictionary<string, long> _bigDictionaryTransaction72; // 719 999 999 - 729 999 999
        private Dictionary<string, long> _bigDictionaryTransaction73; // 729 999 999 - 739 999 999
        private Dictionary<string, long> _bigDictionaryTransaction74; // 739 999 999 - 749 999 999
        private Dictionary<string, long> _bigDictionaryTransaction75; // 749 999 999 - 759 999 999
        private Dictionary<string, long> _bigDictionaryTransaction76; // 759 999 999 - 769 999 999
        private Dictionary<string, long> _bigDictionaryTransaction77; // 769 999 999 - 779 999 999
        private Dictionary<string, long> _bigDictionaryTransaction78; // 779 999 999 - 789 999 999
        private Dictionary<string, long> _bigDictionaryTransaction79; // 789 999 999 - 799 999 999
        private Dictionary<string, long> _bigDictionaryTransaction80; // 799 999 999 - 809 999 999 

        private Dictionary<string, long> _bigDictionaryTransaction81; // 809 999 999 - 819 999 999
        private Dictionary<string, long> _bigDictionaryTransaction82; // 819 999 999 - 829 999 999
        private Dictionary<string, long> _bigDictionaryTransaction83; // 829 999 999 - 839 999 999
        private Dictionary<string, long> _bigDictionaryTransaction84; // 839 999 999 - 849 999 999
        private Dictionary<string, long> _bigDictionaryTransaction85; // 849 999 999 - 859 999 999
        private Dictionary<string, long> _bigDictionaryTransaction86; // 859 999 999 - 869 999 999
        private Dictionary<string, long> _bigDictionaryTransaction87; // 869 999 999 - 879 999 999
        private Dictionary<string, long> _bigDictionaryTransaction88; // 879 999 999 - 889 999 999
        private Dictionary<string, long> _bigDictionaryTransaction89; // 889 999 999 - 899 999 999
        private Dictionary<string, long> _bigDictionaryTransaction90; // 899 999 999 - 909 999 999 

        private Dictionary<string, long> _bigDictionaryTransaction91; // 909 999 999 - 919 999 999
        private Dictionary<string, long> _bigDictionaryTransaction92; // 919 999 999 - 929 999 999
        private Dictionary<string, long> _bigDictionaryTransaction93; // 929 999 999 - 939 999 999
        private Dictionary<string, long> _bigDictionaryTransaction94; // 939 999 999 - 949 999 999
        private Dictionary<string, long> _bigDictionaryTransaction95; // 949 999 999 - 959 999 999
        private Dictionary<string, long> _bigDictionaryTransaction96; // 959 999 999 - 969 999 999
        private Dictionary<string, long> _bigDictionaryTransaction97; // 969 999 999 - 979 999 999
        private Dictionary<string, long> _bigDictionaryTransaction98; // 979 999 999 - 989 999 999
        private Dictionary<string, long> _bigDictionaryTransaction99; // 989 999 999 - 999 999 999
        private Dictionary<string, long> _bigDictionaryTransaction100; // 999 999 999 - 1 009 999 999 

        public const int MaxTransactionHashPerDictionary = 10000000; // 10 millions of transactions hash per dictionary

        /// <summary>
        /// Constructor
        /// </summary>
        public BigDictionaryTransactionHash()
        {
            _bigDictionaryTransaction1 = new Dictionary<string, long>();
            _bigDictionaryTransaction2 = new Dictionary<string, long>();
            _bigDictionaryTransaction3 = new Dictionary<string, long>();
            _bigDictionaryTransaction4 = new Dictionary<string, long>();
            _bigDictionaryTransaction5 = new Dictionary<string, long>();
            _bigDictionaryTransaction6 = new Dictionary<string, long>();
            _bigDictionaryTransaction7 = new Dictionary<string, long>();
            _bigDictionaryTransaction8 = new Dictionary<string, long>();
            _bigDictionaryTransaction9 = new Dictionary<string, long>();
            _bigDictionaryTransaction10 = new Dictionary<string, long>();

            _bigDictionaryTransaction11 = new Dictionary<string, long>();
            _bigDictionaryTransaction12 = new Dictionary<string, long>();
            _bigDictionaryTransaction13 = new Dictionary<string, long>();
            _bigDictionaryTransaction14 = new Dictionary<string, long>();
            _bigDictionaryTransaction15 = new Dictionary<string, long>();
            _bigDictionaryTransaction16 = new Dictionary<string, long>();
            _bigDictionaryTransaction17 = new Dictionary<string, long>();
            _bigDictionaryTransaction18 = new Dictionary<string, long>();
            _bigDictionaryTransaction19 = new Dictionary<string, long>();
            _bigDictionaryTransaction20 = new Dictionary<string, long>();

            _bigDictionaryTransaction21 = new Dictionary<string, long>();
            _bigDictionaryTransaction22 = new Dictionary<string, long>();
            _bigDictionaryTransaction23 = new Dictionary<string, long>();
            _bigDictionaryTransaction24 = new Dictionary<string, long>();
            _bigDictionaryTransaction25 = new Dictionary<string, long>();
            _bigDictionaryTransaction26 = new Dictionary<string, long>();
            _bigDictionaryTransaction27 = new Dictionary<string, long>();
            _bigDictionaryTransaction28 = new Dictionary<string, long>();
            _bigDictionaryTransaction29 = new Dictionary<string, long>();
            _bigDictionaryTransaction30 = new Dictionary<string, long>();

            _bigDictionaryTransaction31 = new Dictionary<string, long>();
            _bigDictionaryTransaction32 = new Dictionary<string, long>();
            _bigDictionaryTransaction33 = new Dictionary<string, long>();
            _bigDictionaryTransaction34 = new Dictionary<string, long>();
            _bigDictionaryTransaction35 = new Dictionary<string, long>();
            _bigDictionaryTransaction36 = new Dictionary<string, long>();
            _bigDictionaryTransaction37 = new Dictionary<string, long>();
            _bigDictionaryTransaction38 = new Dictionary<string, long>();
            _bigDictionaryTransaction39 = new Dictionary<string, long>();
            _bigDictionaryTransaction40 = new Dictionary<string, long>();

            _bigDictionaryTransaction41 = new Dictionary<string, long>();
            _bigDictionaryTransaction42 = new Dictionary<string, long>();
            _bigDictionaryTransaction43 = new Dictionary<string, long>();
            _bigDictionaryTransaction44 = new Dictionary<string, long>();
            _bigDictionaryTransaction45 = new Dictionary<string, long>();
            _bigDictionaryTransaction46 = new Dictionary<string, long>();
            _bigDictionaryTransaction47 = new Dictionary<string, long>();
            _bigDictionaryTransaction48 = new Dictionary<string, long>();
            _bigDictionaryTransaction49 = new Dictionary<string, long>();
            _bigDictionaryTransaction50 = new Dictionary<string, long>();

            _bigDictionaryTransaction51 = new Dictionary<string, long>();
            _bigDictionaryTransaction52 = new Dictionary<string, long>();
            _bigDictionaryTransaction53 = new Dictionary<string, long>();
            _bigDictionaryTransaction54 = new Dictionary<string, long>();
            _bigDictionaryTransaction55 = new Dictionary<string, long>();
            _bigDictionaryTransaction56 = new Dictionary<string, long>();
            _bigDictionaryTransaction57 = new Dictionary<string, long>();
            _bigDictionaryTransaction58 = new Dictionary<string, long>();
            _bigDictionaryTransaction59 = new Dictionary<string, long>();
            _bigDictionaryTransaction60 = new Dictionary<string, long>();

            _bigDictionaryTransaction61 = new Dictionary<string, long>();
            _bigDictionaryTransaction62 = new Dictionary<string, long>();
            _bigDictionaryTransaction63 = new Dictionary<string, long>();
            _bigDictionaryTransaction64 = new Dictionary<string, long>();
            _bigDictionaryTransaction65 = new Dictionary<string, long>();
            _bigDictionaryTransaction66 = new Dictionary<string, long>();
            _bigDictionaryTransaction67 = new Dictionary<string, long>();
            _bigDictionaryTransaction68 = new Dictionary<string, long>();
            _bigDictionaryTransaction69 = new Dictionary<string, long>();
            _bigDictionaryTransaction70 = new Dictionary<string, long>();

            _bigDictionaryTransaction71 = new Dictionary<string, long>();
            _bigDictionaryTransaction72 = new Dictionary<string, long>();
            _bigDictionaryTransaction73 = new Dictionary<string, long>();
            _bigDictionaryTransaction74 = new Dictionary<string, long>();
            _bigDictionaryTransaction75 = new Dictionary<string, long>();
            _bigDictionaryTransaction76 = new Dictionary<string, long>();
            _bigDictionaryTransaction77 = new Dictionary<string, long>();
            _bigDictionaryTransaction78 = new Dictionary<string, long>();
            _bigDictionaryTransaction79 = new Dictionary<string, long>();
            _bigDictionaryTransaction80 = new Dictionary<string, long>();

            _bigDictionaryTransaction81 = new Dictionary<string, long>();
            _bigDictionaryTransaction82 = new Dictionary<string, long>();
            _bigDictionaryTransaction83 = new Dictionary<string, long>();
            _bigDictionaryTransaction84 = new Dictionary<string, long>();
            _bigDictionaryTransaction85 = new Dictionary<string, long>();
            _bigDictionaryTransaction86 = new Dictionary<string, long>();
            _bigDictionaryTransaction87 = new Dictionary<string, long>();
            _bigDictionaryTransaction88 = new Dictionary<string, long>();
            _bigDictionaryTransaction89 = new Dictionary<string, long>();
            _bigDictionaryTransaction90 = new Dictionary<string, long>();

            _bigDictionaryTransaction91 = new Dictionary<string, long>();
            _bigDictionaryTransaction92 = new Dictionary<string, long>();
            _bigDictionaryTransaction93 = new Dictionary<string, long>();
            _bigDictionaryTransaction94 = new Dictionary<string, long>();
            _bigDictionaryTransaction95 = new Dictionary<string, long>();
            _bigDictionaryTransaction96 = new Dictionary<string, long>();
            _bigDictionaryTransaction97 = new Dictionary<string, long>();
            _bigDictionaryTransaction98 = new Dictionary<string, long>();
            _bigDictionaryTransaction99 = new Dictionary<string, long>();
            _bigDictionaryTransaction100 = new Dictionary<string, long>();
        }

        public long Count
        {
            get
            {
                return CountTransaction();
            }
        }

        /// <summary>
        /// Insert transaction
        /// </summary>
        /// <param name="id"></param>
        /// <param name="transaction"></param>
        public void InsertTransactionHash(long id, string transaction)
        {
            long idDictionary = (long)(Math.Ceiling((double)id / MaxTransactionHashPerDictionary));
            if (idDictionary > 0)
            {
                idDictionary -= 1;
            }
            switch (idDictionary)
            {
                case 0:
                    _bigDictionaryTransaction1.Add(transaction, id);
                    break;
                case 1:
                    _bigDictionaryTransaction2.Add(transaction, id);
                    break;
                case 2:
                    _bigDictionaryTransaction3.Add(transaction, id);
                    break;
                case 3:
                    _bigDictionaryTransaction4.Add(transaction, id);
                    break;
                case 4:
                    _bigDictionaryTransaction5.Add(transaction, id);
                    break;
                case 5:
                    _bigDictionaryTransaction6.Add(transaction, id);
                    break;
                case 6:
                    _bigDictionaryTransaction7.Add(transaction, id);
                    break;
                case 7:
                    _bigDictionaryTransaction8.Add(transaction, id);
                    break;
                case 8:
                    _bigDictionaryTransaction9.Add(transaction, id);
                    break;
                case 9:
                    _bigDictionaryTransaction10.Add(transaction, id);
                    break;
                case 10:
                    _bigDictionaryTransaction11.Add(transaction, id);
                    break;
                case 11:
                    _bigDictionaryTransaction12.Add(transaction, id);
                    break;
                case 12:
                    _bigDictionaryTransaction13.Add(transaction, id);
                    break;
                case 13:
                    _bigDictionaryTransaction14.Add(transaction, id);
                    break;
                case 14:
                    _bigDictionaryTransaction15.Add(transaction, id);
                    break;
                case 15:
                    _bigDictionaryTransaction16.Add(transaction, id);
                    break;
                case 16:
                    _bigDictionaryTransaction17.Add(transaction, id);
                    break;
                case 17:
                    _bigDictionaryTransaction18.Add(transaction, id);
                    break;
                case 18:
                    _bigDictionaryTransaction19.Add(transaction, id);
                    break;
                case 19:
                    _bigDictionaryTransaction20.Add(transaction, id);
                    break;
                case 20:
                    _bigDictionaryTransaction21.Add(transaction, id);
                    break;
                case 21:
                    _bigDictionaryTransaction22.Add(transaction, id);
                    break;
                case 22:
                    _bigDictionaryTransaction23.Add(transaction, id);
                    break;
                case 23:
                    _bigDictionaryTransaction24.Add(transaction, id);
                    break;
                case 24:
                    _bigDictionaryTransaction25.Add(transaction, id);
                    break;
                case 25:
                    _bigDictionaryTransaction26.Add(transaction, id);
                    break;
                case 26:
                    _bigDictionaryTransaction27.Add(transaction, id);
                    break;
                case 27:
                    _bigDictionaryTransaction28.Add(transaction, id);
                    break;
                case 28:
                    _bigDictionaryTransaction29.Add(transaction, id);
                    break;
                case 29:
                    _bigDictionaryTransaction30.Add(transaction, id);
                    break;
                case 30:
                    _bigDictionaryTransaction31.Add(transaction, id);
                    break;
                case 31:
                    _bigDictionaryTransaction32.Add(transaction, id);
                    break;
                case 32:
                    _bigDictionaryTransaction33.Add(transaction, id);
                    break;
                case 33:
                    _bigDictionaryTransaction34.Add(transaction, id);
                    break;
                case 34:
                    _bigDictionaryTransaction35.Add(transaction, id);
                    break;
                case 35:
                    _bigDictionaryTransaction36.Add(transaction, id);
                    break;
                case 36:
                    _bigDictionaryTransaction37.Add(transaction, id);
                    break;
                case 37:
                    _bigDictionaryTransaction38.Add(transaction, id);
                    break;
                case 38:
                    _bigDictionaryTransaction39.Add(transaction, id);
                    break;
                case 39:
                    _bigDictionaryTransaction40.Add(transaction, id);
                    break;
                case 40:
                    _bigDictionaryTransaction41.Add(transaction, id);
                    break;
                case 41:
                    _bigDictionaryTransaction42.Add(transaction, id);
                    break;
                case 42:
                    _bigDictionaryTransaction43.Add(transaction, id);
                    break;
                case 43:
                    _bigDictionaryTransaction44.Add(transaction, id);
                    break;
                case 44:
                    _bigDictionaryTransaction45.Add(transaction, id);
                    break;
                case 45:
                    _bigDictionaryTransaction46.Add(transaction, id);
                    break;
                case 46:
                    _bigDictionaryTransaction47.Add(transaction, id);
                    break;
                case 47:
                    _bigDictionaryTransaction48.Add(transaction, id);
                    break;
                case 48:
                    _bigDictionaryTransaction49.Add(transaction, id);
                    break;
                case 49:
                    _bigDictionaryTransaction50.Add(transaction, id);
                    break;
                case 50:
                    _bigDictionaryTransaction51.Add(transaction, id);
                    break;
                case 51:
                    _bigDictionaryTransaction52.Add(transaction, id);
                    break;
                case 52:
                    _bigDictionaryTransaction53.Add(transaction, id);
                    break;
                case 53:
                    _bigDictionaryTransaction54.Add(transaction, id);
                    break;
                case 54:
                    _bigDictionaryTransaction55.Add(transaction, id);
                    break;
                case 55:
                    _bigDictionaryTransaction56.Add(transaction, id);
                    break;
                case 56:
                    _bigDictionaryTransaction57.Add(transaction, id);
                    break;
                case 57:
                    _bigDictionaryTransaction58.Add(transaction, id);
                    break;
                case 58:
                    _bigDictionaryTransaction59.Add(transaction, id);
                    break;
                case 59:
                    _bigDictionaryTransaction60.Add(transaction, id);
                    break;
                case 60:
                    _bigDictionaryTransaction61.Add(transaction, id);
                    break;
                case 61:
                    _bigDictionaryTransaction62.Add(transaction, id);
                    break;
                case 62:
                    _bigDictionaryTransaction63.Add(transaction, id);
                    break;
                case 63:
                    _bigDictionaryTransaction64.Add(transaction, id);
                    break;
                case 64:
                    _bigDictionaryTransaction65.Add(transaction, id);
                    break;
                case 65:
                    _bigDictionaryTransaction66.Add(transaction, id);
                    break;
                case 66:
                    _bigDictionaryTransaction67.Add(transaction, id);
                    break;
                case 67:
                    _bigDictionaryTransaction68.Add(transaction, id);
                    break;
                case 68:
                    _bigDictionaryTransaction69.Add(transaction, id);
                    break;
                case 69:
                    _bigDictionaryTransaction70.Add(transaction, id);
                    break;
                case 70:
                    _bigDictionaryTransaction71.Add(transaction, id);
                    break;
                case 71:
                    _bigDictionaryTransaction72.Add(transaction, id);
                    break;
                case 72:
                    _bigDictionaryTransaction73.Add(transaction, id);
                    break;
                case 73:
                    _bigDictionaryTransaction74.Add(transaction, id);
                    break;
                case 74:
                    _bigDictionaryTransaction75.Add(transaction, id);
                    break;
                case 75:
                    _bigDictionaryTransaction76.Add(transaction, id);
                    break;
                case 76:
                    _bigDictionaryTransaction77.Add(transaction, id);
                    break;
                case 77:
                    _bigDictionaryTransaction78.Add(transaction, id);
                    break;
                case 78:
                    _bigDictionaryTransaction79.Add(transaction, id);
                    break;
                case 79:
                    _bigDictionaryTransaction80.Add(transaction, id);
                    break;
                case 80:
                    _bigDictionaryTransaction81.Add(transaction, id);
                    break;
                case 81:
                    _bigDictionaryTransaction82.Add(transaction, id);
                    break;
                case 82:
                    _bigDictionaryTransaction83.Add(transaction, id);
                    break;
                case 83:
                    _bigDictionaryTransaction84.Add(transaction, id);
                    break;
                case 84:
                    _bigDictionaryTransaction85.Add(transaction, id);
                    break;
                case 85:
                    _bigDictionaryTransaction86.Add(transaction, id);
                    break;
                case 86:
                    _bigDictionaryTransaction87.Add(transaction, id);
                    break;
                case 87:
                    _bigDictionaryTransaction88.Add(transaction, id);
                    break;
                case 88:
                    _bigDictionaryTransaction89.Add(transaction, id);
                    break;
                case 89:
                    _bigDictionaryTransaction90.Add(transaction, id);
                    break;
                case 90:
                    _bigDictionaryTransaction91.Add(transaction, id);
                    break;
                case 91:
                    _bigDictionaryTransaction92.Add(transaction, id);
                    break;
                case 92:
                    _bigDictionaryTransaction93.Add(transaction, id);
                    break;
                case 93:
                    _bigDictionaryTransaction94.Add(transaction, id);
                    break;
                case 94:
                    _bigDictionaryTransaction95.Add(transaction, id);
                    break;
                case 95:
                    _bigDictionaryTransaction96.Add(transaction, id);
                    break;
                case 96:
                    _bigDictionaryTransaction97.Add(transaction, id);
                    break;
                case 97:
                    _bigDictionaryTransaction98.Add(transaction, id);
                    break;
                case 98:
                    _bigDictionaryTransaction99.Add(transaction, id);
                    break;
                case 99:
                    _bigDictionaryTransaction100.Add(transaction, id);
                    break;
            }
        }

        /// <summary>
        /// Retrieve total transaction saved.
        /// </summary>
        /// <returns></returns>
        public long CountTransaction()
        {
            return _bigDictionaryTransaction1.Count +
                _bigDictionaryTransaction2.Count +
                _bigDictionaryTransaction3.Count +
                _bigDictionaryTransaction4.Count +
                _bigDictionaryTransaction5.Count +
                _bigDictionaryTransaction6.Count +
                _bigDictionaryTransaction7.Count +
                _bigDictionaryTransaction8.Count +
                _bigDictionaryTransaction9.Count +
                _bigDictionaryTransaction10.Count +
                _bigDictionaryTransaction11.Count +
                _bigDictionaryTransaction12.Count +
                _bigDictionaryTransaction13.Count +
                _bigDictionaryTransaction14.Count +
                _bigDictionaryTransaction15.Count +
                _bigDictionaryTransaction16.Count +
                _bigDictionaryTransaction17.Count +
                _bigDictionaryTransaction18.Count +
                _bigDictionaryTransaction19.Count +
                _bigDictionaryTransaction20.Count +
                _bigDictionaryTransaction21.Count +
                _bigDictionaryTransaction22.Count +
                _bigDictionaryTransaction23.Count +
                _bigDictionaryTransaction24.Count +
                _bigDictionaryTransaction25.Count +
                _bigDictionaryTransaction26.Count +
                _bigDictionaryTransaction27.Count +
                _bigDictionaryTransaction28.Count +
                _bigDictionaryTransaction29.Count +
                _bigDictionaryTransaction30.Count +
                _bigDictionaryTransaction31.Count +
                _bigDictionaryTransaction32.Count +
                _bigDictionaryTransaction33.Count +
                _bigDictionaryTransaction34.Count +
                _bigDictionaryTransaction35.Count +
                _bigDictionaryTransaction36.Count +
                _bigDictionaryTransaction37.Count +
                _bigDictionaryTransaction38.Count +
                _bigDictionaryTransaction39.Count +
                _bigDictionaryTransaction40.Count +
                _bigDictionaryTransaction41.Count +
                _bigDictionaryTransaction42.Count +
                _bigDictionaryTransaction43.Count +
                _bigDictionaryTransaction44.Count +
                _bigDictionaryTransaction45.Count +
                _bigDictionaryTransaction46.Count +
                _bigDictionaryTransaction47.Count +
                _bigDictionaryTransaction48.Count +
                _bigDictionaryTransaction49.Count +
                _bigDictionaryTransaction50.Count +
                _bigDictionaryTransaction51.Count +
                _bigDictionaryTransaction52.Count +
                _bigDictionaryTransaction53.Count +
                _bigDictionaryTransaction54.Count +
                _bigDictionaryTransaction55.Count +
                _bigDictionaryTransaction56.Count +
                _bigDictionaryTransaction57.Count +
                _bigDictionaryTransaction58.Count +
                _bigDictionaryTransaction59.Count +
                _bigDictionaryTransaction60.Count +
                _bigDictionaryTransaction61.Count +
                _bigDictionaryTransaction62.Count +
                _bigDictionaryTransaction63.Count +
                _bigDictionaryTransaction64.Count +
                _bigDictionaryTransaction65.Count +
                _bigDictionaryTransaction66.Count +
                _bigDictionaryTransaction67.Count +
                _bigDictionaryTransaction68.Count +
                _bigDictionaryTransaction69.Count +
                _bigDictionaryTransaction70.Count +
                _bigDictionaryTransaction71.Count +
                _bigDictionaryTransaction72.Count +
                _bigDictionaryTransaction73.Count +
                _bigDictionaryTransaction74.Count +
                _bigDictionaryTransaction75.Count +
                _bigDictionaryTransaction76.Count +
                _bigDictionaryTransaction77.Count +
                _bigDictionaryTransaction78.Count +
                _bigDictionaryTransaction79.Count +
                _bigDictionaryTransaction80.Count +
                _bigDictionaryTransaction81.Count +
                _bigDictionaryTransaction82.Count +
                _bigDictionaryTransaction83.Count +
                _bigDictionaryTransaction84.Count +
                _bigDictionaryTransaction85.Count +
                _bigDictionaryTransaction86.Count +
                _bigDictionaryTransaction87.Count +
                _bigDictionaryTransaction88.Count +
                _bigDictionaryTransaction89.Count +
                _bigDictionaryTransaction90.Count +
                _bigDictionaryTransaction91.Count +
                _bigDictionaryTransaction92.Count +
                _bigDictionaryTransaction93.Count +
                _bigDictionaryTransaction94.Count +
                _bigDictionaryTransaction95.Count +
                _bigDictionaryTransaction96.Count +
                _bigDictionaryTransaction97.Count +
                _bigDictionaryTransaction98.Count +
                _bigDictionaryTransaction99.Count +
                _bigDictionaryTransaction100.Count;
        }

        /// <summary>
        /// Check on every dictionary the hash existance.
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public long ContainsKey(string hash)
        {
            if (_bigDictionaryTransaction1.ContainsKey(hash))
            {
                return _bigDictionaryTransaction1[hash];
            }
            if (_bigDictionaryTransaction2.ContainsKey(hash))
            {
                return _bigDictionaryTransaction2[hash];
            }
            if (_bigDictionaryTransaction3.ContainsKey(hash))
            {
                return _bigDictionaryTransaction3[hash];
            }
            if (_bigDictionaryTransaction4.ContainsKey(hash))
            {
                return _bigDictionaryTransaction4[hash];
            }
            if (_bigDictionaryTransaction5.ContainsKey(hash))
            {
                return _bigDictionaryTransaction5[hash];
            }
            if (_bigDictionaryTransaction6.ContainsKey(hash))
            {
                return _bigDictionaryTransaction6[hash];
            }
            if (_bigDictionaryTransaction7.ContainsKey(hash))
            {
                return _bigDictionaryTransaction7[hash];
            }
            if (_bigDictionaryTransaction8.ContainsKey(hash))
            {
                return _bigDictionaryTransaction8[hash];
            }
            if (_bigDictionaryTransaction9.ContainsKey(hash))
            {
                return _bigDictionaryTransaction9[hash];
            }
            if (_bigDictionaryTransaction10.ContainsKey(hash))
            {
                return _bigDictionaryTransaction10[hash];
            }
            if (_bigDictionaryTransaction11.ContainsKey(hash))
            {
                return _bigDictionaryTransaction11[hash];
            }
            if (_bigDictionaryTransaction12.ContainsKey(hash))
            {
                return _bigDictionaryTransaction12[hash];
            }
            if (_bigDictionaryTransaction13.ContainsKey(hash))
            {
                return _bigDictionaryTransaction13[hash];
            }
            if (_bigDictionaryTransaction14.ContainsKey(hash))
            {
                return _bigDictionaryTransaction14[hash];
            }
            if (_bigDictionaryTransaction15.ContainsKey(hash))
            {
                return _bigDictionaryTransaction15[hash];
            }
            if (_bigDictionaryTransaction16.ContainsKey(hash))
            {
                return _bigDictionaryTransaction16[hash];
            }
            if (_bigDictionaryTransaction17.ContainsKey(hash))
            {
                return _bigDictionaryTransaction17[hash];
            }
            if (_bigDictionaryTransaction18.ContainsKey(hash))
            {
                return _bigDictionaryTransaction18[hash];
            }
            if (_bigDictionaryTransaction19.ContainsKey(hash))
            {
                return _bigDictionaryTransaction19[hash];
            }
            if (_bigDictionaryTransaction20.ContainsKey(hash))
            {
                return _bigDictionaryTransaction20[hash];
            }
            if (_bigDictionaryTransaction21.ContainsKey(hash))
            {
                return _bigDictionaryTransaction21[hash];
            }
            if (_bigDictionaryTransaction22.ContainsKey(hash))
            {
                return _bigDictionaryTransaction22[hash];
            }
            if (_bigDictionaryTransaction23.ContainsKey(hash))
            {
                return _bigDictionaryTransaction23[hash];
            }
            if (_bigDictionaryTransaction24.ContainsKey(hash))
            {
                return _bigDictionaryTransaction24[hash];
            }
            if (_bigDictionaryTransaction25.ContainsKey(hash))
            {
                return _bigDictionaryTransaction25[hash];
            }
            if (_bigDictionaryTransaction26.ContainsKey(hash))
            {
                return _bigDictionaryTransaction26[hash];
            }
            if (_bigDictionaryTransaction27.ContainsKey(hash))
            {
                return _bigDictionaryTransaction27[hash];
            }
            if (_bigDictionaryTransaction28.ContainsKey(hash))
            {
                return _bigDictionaryTransaction28[hash];
            }
            if (_bigDictionaryTransaction29.ContainsKey(hash))
            {
                return _bigDictionaryTransaction29[hash];
            }
            if (_bigDictionaryTransaction30.ContainsKey(hash))
            {
                return _bigDictionaryTransaction30[hash];
            }
            if (_bigDictionaryTransaction31.ContainsKey(hash))
            {
                return _bigDictionaryTransaction31[hash];
            }
            if (_bigDictionaryTransaction32.ContainsKey(hash))
            {
                return _bigDictionaryTransaction33[hash];
            }
            if (_bigDictionaryTransaction33.ContainsKey(hash))
            {
                return _bigDictionaryTransaction33[hash];
            }
            if (_bigDictionaryTransaction34.ContainsKey(hash))
            {
                return _bigDictionaryTransaction34[hash];
            }
            if (_bigDictionaryTransaction35.ContainsKey(hash))
            {
                return _bigDictionaryTransaction35[hash];
            }
            if (_bigDictionaryTransaction36.ContainsKey(hash))
            {
                return _bigDictionaryTransaction36[hash];
            }
            if (_bigDictionaryTransaction37.ContainsKey(hash))
            {
                return _bigDictionaryTransaction37[hash];
            }
            if (_bigDictionaryTransaction38.ContainsKey(hash))
            {
                return _bigDictionaryTransaction38[hash];
            }
            if (_bigDictionaryTransaction39.ContainsKey(hash))
            {
                return _bigDictionaryTransaction39[hash];
            }
            if (_bigDictionaryTransaction40.ContainsKey(hash))
            {
                return _bigDictionaryTransaction40[hash];
            }
            if (_bigDictionaryTransaction41.ContainsKey(hash))
            {
                return _bigDictionaryTransaction41[hash];
            }
            if (_bigDictionaryTransaction42.ContainsKey(hash))
            {
                return _bigDictionaryTransaction42[hash];
            }
            if (_bigDictionaryTransaction43.ContainsKey(hash))
            {
                return _bigDictionaryTransaction43[hash];
            }
            if (_bigDictionaryTransaction44.ContainsKey(hash))
            {
                return _bigDictionaryTransaction44[hash];
            }
            if (_bigDictionaryTransaction45.ContainsKey(hash))
            {
                return _bigDictionaryTransaction45[hash];
            }
            if (_bigDictionaryTransaction46.ContainsKey(hash))
            {
                return _bigDictionaryTransaction46[hash];
            }
            if (_bigDictionaryTransaction47.ContainsKey(hash))
            {
                return _bigDictionaryTransaction47[hash];
            }
            if (_bigDictionaryTransaction48.ContainsKey(hash))
            {
                return _bigDictionaryTransaction48[hash];
            }
            if (_bigDictionaryTransaction49.ContainsKey(hash))
            {
                return _bigDictionaryTransaction49[hash];
            }
            if (_bigDictionaryTransaction50.ContainsKey(hash))
            {
                return _bigDictionaryTransaction50[hash];
            }
            if (_bigDictionaryTransaction51.ContainsKey(hash))
            {
                return _bigDictionaryTransaction51[hash];
            }
            if (_bigDictionaryTransaction52.ContainsKey(hash))
            {
                return _bigDictionaryTransaction52[hash];
            }
            if (_bigDictionaryTransaction53.ContainsKey(hash))
            {
                return _bigDictionaryTransaction53[hash];
            }
            if (_bigDictionaryTransaction54.ContainsKey(hash))
            {
                return _bigDictionaryTransaction54[hash];
            }
            if (_bigDictionaryTransaction55.ContainsKey(hash))
            {
                return _bigDictionaryTransaction55[hash];
            }
            if (_bigDictionaryTransaction56.ContainsKey(hash))
            {
                return _bigDictionaryTransaction56[hash];
            }
            if (_bigDictionaryTransaction57.ContainsKey(hash))
            {
                return _bigDictionaryTransaction57[hash];
            }
            if (_bigDictionaryTransaction58.ContainsKey(hash))
            {
                return _bigDictionaryTransaction58[hash];
            }
            if (_bigDictionaryTransaction59.ContainsKey(hash))
            {
                return _bigDictionaryTransaction59[hash];
            }
            if (_bigDictionaryTransaction60.ContainsKey(hash))
            {
                return _bigDictionaryTransaction60[hash];
            }
            if (_bigDictionaryTransaction61.ContainsKey(hash))
            {
                return _bigDictionaryTransaction61[hash];
            }
            if (_bigDictionaryTransaction62.ContainsKey(hash))
            {
                return _bigDictionaryTransaction62[hash];
            }
            if (_bigDictionaryTransaction63.ContainsKey(hash))
            {
                return _bigDictionaryTransaction63[hash];
            }
            if (_bigDictionaryTransaction64.ContainsKey(hash))
            {
                return _bigDictionaryTransaction64[hash];
            }
            if (_bigDictionaryTransaction65.ContainsKey(hash))
            {
                return _bigDictionaryTransaction65[hash];
            }
            if (_bigDictionaryTransaction66.ContainsKey(hash))
            {
                return _bigDictionaryTransaction66[hash];
            }
            if (_bigDictionaryTransaction67.ContainsKey(hash))
            {
                return _bigDictionaryTransaction67[hash];
            }
            if (_bigDictionaryTransaction68.ContainsKey(hash))
            {
                return _bigDictionaryTransaction68[hash];
            }
            if (_bigDictionaryTransaction69.ContainsKey(hash))
            {
                return _bigDictionaryTransaction69[hash];
            }
            if (_bigDictionaryTransaction70.ContainsKey(hash))
            {
                return _bigDictionaryTransaction70[hash];
            }
            if (_bigDictionaryTransaction71.ContainsKey(hash))
            {
                return _bigDictionaryTransaction71[hash];
            }
            if (_bigDictionaryTransaction72.ContainsKey(hash))
            {
                return _bigDictionaryTransaction72[hash];
            }
            if (_bigDictionaryTransaction73.ContainsKey(hash))
            {
                return _bigDictionaryTransaction73[hash];
            }
            if (_bigDictionaryTransaction74.ContainsKey(hash))
            {
                return _bigDictionaryTransaction74[hash];
            }
            if (_bigDictionaryTransaction75.ContainsKey(hash))
            {
                return _bigDictionaryTransaction75[hash];
            }
            if (_bigDictionaryTransaction76.ContainsKey(hash))
            {
                return _bigDictionaryTransaction76[hash];
            }
            if (_bigDictionaryTransaction77.ContainsKey(hash))
            {
                return _bigDictionaryTransaction77[hash];
            }
            if (_bigDictionaryTransaction78.ContainsKey(hash))
            {
                return _bigDictionaryTransaction78[hash];
            }
            if (_bigDictionaryTransaction79.ContainsKey(hash))
            {
                return _bigDictionaryTransaction79[hash];
            }
            if (_bigDictionaryTransaction80.ContainsKey(hash))
            {
                return _bigDictionaryTransaction80[hash];
            }
            if (_bigDictionaryTransaction81.ContainsKey(hash))
            {
                return _bigDictionaryTransaction81[hash];
            }
            if (_bigDictionaryTransaction82.ContainsKey(hash))
            {
                return _bigDictionaryTransaction82[hash];
            }
            if (_bigDictionaryTransaction83.ContainsKey(hash))
            {
                return _bigDictionaryTransaction83[hash];
            }
            if (_bigDictionaryTransaction84.ContainsKey(hash))
            {
                return _bigDictionaryTransaction84[hash];
            }
            if (_bigDictionaryTransaction85.ContainsKey(hash))
            {
                return _bigDictionaryTransaction85[hash];
            }
            if (_bigDictionaryTransaction86.ContainsKey(hash))
            {
                return _bigDictionaryTransaction86[hash];
            }
            if (_bigDictionaryTransaction87.ContainsKey(hash))
            {
                return _bigDictionaryTransaction87[hash];
            }
            if (_bigDictionaryTransaction88.ContainsKey(hash))
            {
                return _bigDictionaryTransaction88[hash];
            }
            if (_bigDictionaryTransaction89.ContainsKey(hash))
            {
                return _bigDictionaryTransaction89[hash];
            }
            if (_bigDictionaryTransaction90.ContainsKey(hash))
            {
                return _bigDictionaryTransaction90[hash];
            }
            if (_bigDictionaryTransaction91.ContainsKey(hash))
            {
                return _bigDictionaryTransaction91[hash];
            }
            if (_bigDictionaryTransaction92.ContainsKey(hash))
            {
                return _bigDictionaryTransaction92[hash];
            }
            if (_bigDictionaryTransaction93.ContainsKey(hash))
            {
                return _bigDictionaryTransaction93[hash];
            }
            if (_bigDictionaryTransaction94.ContainsKey(hash))
            {
                return _bigDictionaryTransaction94[hash];
            }
            if (_bigDictionaryTransaction95.ContainsKey(hash))
            {
                return _bigDictionaryTransaction95[hash];
            }
            if (_bigDictionaryTransaction96.ContainsKey(hash))
            {
                return _bigDictionaryTransaction96[hash];
            }
            if (_bigDictionaryTransaction97.ContainsKey(hash))
            {
                return _bigDictionaryTransaction97[hash];
            }
            if (_bigDictionaryTransaction98.ContainsKey(hash))
            {
                return _bigDictionaryTransaction98[hash];
            }
            if (_bigDictionaryTransaction99.ContainsKey(hash))
            {
                return _bigDictionaryTransaction99[hash];
            }
            if (_bigDictionaryTransaction100.ContainsKey(hash))
            {
                return _bigDictionaryTransaction100[hash];
            }

            return -1;
        }

        /// <summary>
        /// Clear dictionnary
        /// </summary>
        public void Clear()
        {
            _bigDictionaryTransaction1.Clear();
            _bigDictionaryTransaction2.Clear();
            _bigDictionaryTransaction3.Clear();
            _bigDictionaryTransaction4.Clear();
            _bigDictionaryTransaction5.Clear();
            _bigDictionaryTransaction6.Clear();
            _bigDictionaryTransaction7.Clear();
            _bigDictionaryTransaction8.Clear();
            _bigDictionaryTransaction9.Clear();
            _bigDictionaryTransaction10.Clear();
            _bigDictionaryTransaction11.Clear();
            _bigDictionaryTransaction12.Clear();
            _bigDictionaryTransaction13.Clear();
            _bigDictionaryTransaction14.Clear();
            _bigDictionaryTransaction15.Clear();
            _bigDictionaryTransaction16.Clear();
            _bigDictionaryTransaction17.Clear();
            _bigDictionaryTransaction18.Clear();
            _bigDictionaryTransaction19.Clear();
            _bigDictionaryTransaction20.Clear();
            _bigDictionaryTransaction21.Clear();
            _bigDictionaryTransaction22.Clear();
            _bigDictionaryTransaction23.Clear();
            _bigDictionaryTransaction24.Clear();
            _bigDictionaryTransaction25.Clear();
            _bigDictionaryTransaction26.Clear();
            _bigDictionaryTransaction27.Clear();
            _bigDictionaryTransaction28.Clear();
            _bigDictionaryTransaction29.Clear();
            _bigDictionaryTransaction30.Clear();
            _bigDictionaryTransaction31.Clear();
            _bigDictionaryTransaction32.Clear();
            _bigDictionaryTransaction33.Clear();
            _bigDictionaryTransaction34.Clear();
            _bigDictionaryTransaction35.Clear();
            _bigDictionaryTransaction36.Clear();
            _bigDictionaryTransaction37.Clear();
            _bigDictionaryTransaction38.Clear();
            _bigDictionaryTransaction39.Clear();
            _bigDictionaryTransaction40.Clear();
            _bigDictionaryTransaction41.Clear();
            _bigDictionaryTransaction42.Clear();
            _bigDictionaryTransaction43.Clear();
            _bigDictionaryTransaction44.Clear();
            _bigDictionaryTransaction45.Clear();
            _bigDictionaryTransaction46.Clear();
            _bigDictionaryTransaction47.Clear();
            _bigDictionaryTransaction48.Clear();
            _bigDictionaryTransaction49.Clear();
            _bigDictionaryTransaction50.Clear();
            _bigDictionaryTransaction51.Clear();
            _bigDictionaryTransaction52.Clear();
            _bigDictionaryTransaction53.Clear();
            _bigDictionaryTransaction54.Clear();
            _bigDictionaryTransaction55.Clear();
            _bigDictionaryTransaction56.Clear();
            _bigDictionaryTransaction57.Clear();
            _bigDictionaryTransaction58.Clear();
            _bigDictionaryTransaction59.Clear();
            _bigDictionaryTransaction60.Clear();
            _bigDictionaryTransaction61.Clear();
            _bigDictionaryTransaction62.Clear();
            _bigDictionaryTransaction63.Clear();
            _bigDictionaryTransaction64.Clear();
            _bigDictionaryTransaction65.Clear();
            _bigDictionaryTransaction66.Clear();
            _bigDictionaryTransaction67.Clear();
            _bigDictionaryTransaction68.Clear();
            _bigDictionaryTransaction69.Clear();
            _bigDictionaryTransaction70.Clear();
            _bigDictionaryTransaction71.Clear();
            _bigDictionaryTransaction72.Clear();
            _bigDictionaryTransaction73.Clear();
            _bigDictionaryTransaction74.Clear();
            _bigDictionaryTransaction75.Clear();
            _bigDictionaryTransaction76.Clear();
            _bigDictionaryTransaction77.Clear();
            _bigDictionaryTransaction78.Clear();
            _bigDictionaryTransaction79.Clear();
            _bigDictionaryTransaction80.Clear();
            _bigDictionaryTransaction81.Clear();
            _bigDictionaryTransaction82.Clear();
            _bigDictionaryTransaction83.Clear();
            _bigDictionaryTransaction84.Clear();
            _bigDictionaryTransaction85.Clear();
            _bigDictionaryTransaction86.Clear();
            _bigDictionaryTransaction87.Clear();
            _bigDictionaryTransaction88.Clear();
            _bigDictionaryTransaction89.Clear();
            _bigDictionaryTransaction90.Clear();
            _bigDictionaryTransaction91.Clear();
            _bigDictionaryTransaction92.Clear();
            _bigDictionaryTransaction93.Clear();
            _bigDictionaryTransaction94.Clear();
            _bigDictionaryTransaction95.Clear();
            _bigDictionaryTransaction96.Clear();
            _bigDictionaryTransaction97.Clear();
            _bigDictionaryTransaction98.Clear();
            _bigDictionaryTransaction99.Clear();
            _bigDictionaryTransaction100.Clear();
        }
    }
}
