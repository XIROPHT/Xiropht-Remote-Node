using System;
using System.Collections.Generic;


namespace Xiropht_RemoteNode.Object
{
    public class BigDictionaryTransactionSortedPerWallet
    {
        private Dictionary<float, List<string>> _bigDictionaryTransaction1;
        private Dictionary<float, List<string>> _bigDictionaryTransaction2;
        private Dictionary<float, List<string>> _bigDictionaryTransaction3;
        private Dictionary<float, List<string>> _bigDictionaryTransaction4;
        private Dictionary<float, List<string>> _bigDictionaryTransaction5;
        private Dictionary<float, List<string>> _bigDictionaryTransaction6;
        private Dictionary<float, List<string>> _bigDictionaryTransaction7;
        private Dictionary<float, List<string>> _bigDictionaryTransaction8;
        private Dictionary<float, List<string>> _bigDictionaryTransaction9;
        private Dictionary<float, List<string>> _bigDictionaryTransaction10;
        private Dictionary<float, List<string>> _bigDictionaryTransaction11;
        private Dictionary<float, List<string>> _bigDictionaryTransaction12;
        private Dictionary<float, List<string>> _bigDictionaryTransaction13;
        private Dictionary<float, List<string>> _bigDictionaryTransaction14;
        private Dictionary<float, List<string>> _bigDictionaryTransaction15;
        private Dictionary<float, List<string>> _bigDictionaryTransaction16;
        private Dictionary<float, List<string>> _bigDictionaryTransaction17;
        private Dictionary<float, List<string>> _bigDictionaryTransaction18;
        private Dictionary<float, List<string>> _bigDictionaryTransaction19;
        private Dictionary<float, List<string>> _bigDictionaryTransaction20;
        private Dictionary<float, List<string>> _bigDictionaryTransaction21;
        private Dictionary<float, List<string>> _bigDictionaryTransaction22;
        private Dictionary<float, List<string>> _bigDictionaryTransaction23;
        private Dictionary<float, List<string>> _bigDictionaryTransaction24;
        private Dictionary<float, List<string>> _bigDictionaryTransaction25;
        private Dictionary<float, List<string>> _bigDictionaryTransaction26;
        private Dictionary<float, List<string>> _bigDictionaryTransaction27;
        private Dictionary<float, List<string>> _bigDictionaryTransaction28;
        private Dictionary<float, List<string>> _bigDictionaryTransaction29;
        private Dictionary<float, List<string>> _bigDictionaryTransaction30;
        private Dictionary<float, List<string>> _bigDictionaryTransaction31;
        private Dictionary<float, List<string>> _bigDictionaryTransaction32;
        private Dictionary<float, List<string>> _bigDictionaryTransaction33;
        private Dictionary<float, List<string>> _bigDictionaryTransaction34;
        private Dictionary<float, List<string>> _bigDictionaryTransaction35;
        private Dictionary<float, List<string>> _bigDictionaryTransaction36;
        private Dictionary<float, List<string>> _bigDictionaryTransaction37;
        private Dictionary<float, List<string>> _bigDictionaryTransaction38;
        private Dictionary<float, List<string>> _bigDictionaryTransaction39;
        private Dictionary<float, List<string>> _bigDictionaryTransaction40;
        private Dictionary<float, List<string>> _bigDictionaryTransaction41;
        private Dictionary<float, List<string>> _bigDictionaryTransaction42;
        private Dictionary<float, List<string>> _bigDictionaryTransaction43;
        private Dictionary<float, List<string>> _bigDictionaryTransaction44;
        private Dictionary<float, List<string>> _bigDictionaryTransaction45;
        private Dictionary<float, List<string>> _bigDictionaryTransaction46;
        private Dictionary<float, List<string>> _bigDictionaryTransaction47;
        private Dictionary<float, List<string>> _bigDictionaryTransaction48;
        private Dictionary<float, List<string>> _bigDictionaryTransaction49;
        private Dictionary<float, List<string>> _bigDictionaryTransaction50;
        private Dictionary<float, List<string>> _bigDictionaryTransaction51;
        private Dictionary<float, List<string>> _bigDictionaryTransaction52;
        private Dictionary<float, List<string>> _bigDictionaryTransaction53;
        private Dictionary<float, List<string>> _bigDictionaryTransaction54;
        private Dictionary<float, List<string>> _bigDictionaryTransaction55;
        private Dictionary<float, List<string>> _bigDictionaryTransaction56;
        private Dictionary<float, List<string>> _bigDictionaryTransaction57;
        private Dictionary<float, List<string>> _bigDictionaryTransaction58;
        private Dictionary<float, List<string>> _bigDictionaryTransaction59;
        private Dictionary<float, List<string>> _bigDictionaryTransaction60;
        private Dictionary<float, List<string>> _bigDictionaryTransaction61;
        private Dictionary<float, List<string>> _bigDictionaryTransaction62;
        private Dictionary<float, List<string>> _bigDictionaryTransaction63;
        private Dictionary<float, List<string>> _bigDictionaryTransaction64;
        private Dictionary<float, List<string>> _bigDictionaryTransaction65;
        private Dictionary<float, List<string>> _bigDictionaryTransaction66;
        private Dictionary<float, List<string>> _bigDictionaryTransaction67;
        private Dictionary<float, List<string>> _bigDictionaryTransaction68;
        private Dictionary<float, List<string>> _bigDictionaryTransaction69;
        private Dictionary<float, List<string>> _bigDictionaryTransaction70;
        private Dictionary<float, List<string>> _bigDictionaryTransaction71;
        private Dictionary<float, List<string>> _bigDictionaryTransaction72;
        private Dictionary<float, List<string>> _bigDictionaryTransaction73;
        private Dictionary<float, List<string>> _bigDictionaryTransaction74;
        private Dictionary<float, List<string>> _bigDictionaryTransaction75;
        private Dictionary<float, List<string>> _bigDictionaryTransaction76;
        private Dictionary<float, List<string>> _bigDictionaryTransaction77;
        private Dictionary<float, List<string>> _bigDictionaryTransaction78;
        private Dictionary<float, List<string>> _bigDictionaryTransaction79;
        private Dictionary<float, List<string>> _bigDictionaryTransaction80;
        private Dictionary<float, List<string>> _bigDictionaryTransaction81;
        private Dictionary<float, List<string>> _bigDictionaryTransaction82;
        private Dictionary<float, List<string>> _bigDictionaryTransaction83;
        private Dictionary<float, List<string>> _bigDictionaryTransaction84;
        private Dictionary<float, List<string>> _bigDictionaryTransaction85;
        private Dictionary<float, List<string>> _bigDictionaryTransaction86;
        private Dictionary<float, List<string>> _bigDictionaryTransaction87;
        private Dictionary<float, List<string>> _bigDictionaryTransaction88;
        private Dictionary<float, List<string>> _bigDictionaryTransaction89;
        private Dictionary<float, List<string>> _bigDictionaryTransaction90;
        private Dictionary<float, List<string>> _bigDictionaryTransaction91;
        private Dictionary<float, List<string>> _bigDictionaryTransaction92;
        private Dictionary<float, List<string>> _bigDictionaryTransaction93;
        private Dictionary<float, List<string>> _bigDictionaryTransaction94;
        private Dictionary<float, List<string>> _bigDictionaryTransaction95;
        private Dictionary<float, List<string>> _bigDictionaryTransaction96;
        private Dictionary<float, List<string>> _bigDictionaryTransaction97;
        private Dictionary<float, List<string>> _bigDictionaryTransaction98;
        private Dictionary<float, List<string>> _bigDictionaryTransaction99;
        private Dictionary<float, List<string>> _bigDictionaryTransaction100;


        public const int MaxTransactionPerDictionary = 10000000; // 10 millions of transactions hash per dictionary


        public BigDictionaryTransactionSortedPerWallet()
        {
            _bigDictionaryTransaction1 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction2 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction3 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction4 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction5 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction6 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction7 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction8 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction9 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction10 = new Dictionary<float, List<string>>();

            _bigDictionaryTransaction11 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction12 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction13 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction14 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction15 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction16 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction17 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction18 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction19 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction20 = new Dictionary<float, List<string>>();

            _bigDictionaryTransaction21 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction22 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction23 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction24 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction25 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction26 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction27 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction28 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction29 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction30 = new Dictionary<float, List<string>>();

            _bigDictionaryTransaction31 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction32 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction33 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction34 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction35 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction36 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction37 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction38 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction39 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction40 = new Dictionary<float, List<string>>();

            _bigDictionaryTransaction41 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction42 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction43 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction44 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction45 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction46 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction47 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction48 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction49 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction50 = new Dictionary<float, List<string>>();

            _bigDictionaryTransaction51 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction52 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction53 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction54 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction55 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction56 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction57 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction58 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction59 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction60 = new Dictionary<float, List<string>>();

            _bigDictionaryTransaction61 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction62 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction63 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction64 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction65 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction66 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction67 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction68 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction69 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction70 = new Dictionary<float, List<string>>();

            _bigDictionaryTransaction71 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction72 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction73 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction74 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction75 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction76 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction77 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction78 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction79 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction80 = new Dictionary<float, List<string>>();

            _bigDictionaryTransaction81 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction82 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction83 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction84 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction85 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction86 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction87 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction88 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction89 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction90 = new Dictionary<float, List<string>>();

            _bigDictionaryTransaction91 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction92 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction93 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction94 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction95 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction96 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction97 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction98 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction99 = new Dictionary<float, List<string>>();
            _bigDictionaryTransaction100 = new Dictionary<float, List<string>>();
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
        public void InsertTransactionSorted(float id, string transaction)
        {
            long idDictionary = ContainsKey(id);
            if (idDictionary != -1)
            {
                switch (idDictionary)
                {
                    case 0:
                        _bigDictionaryTransaction1[id].Add(transaction);
                        break;
                    case 1:
                        _bigDictionaryTransaction2[id].Add(transaction);
                        break;
                    case 2:
                        _bigDictionaryTransaction3[id].Add(transaction);
                        break;
                    case 3:
                        _bigDictionaryTransaction4[id].Add(transaction);
                        break;
                    case 4:
                        _bigDictionaryTransaction5[id].Add(transaction);
                        break;
                    case 5:
                        _bigDictionaryTransaction6[id].Add(transaction);
                        break;
                    case 6:
                        _bigDictionaryTransaction7[id].Add(transaction);
                        break;
                    case 7:
                        _bigDictionaryTransaction8[id].Add(transaction);
                        break;
                    case 8:
                        _bigDictionaryTransaction9[id].Add(transaction);
                        break;
                    case 9:
                        _bigDictionaryTransaction10[id].Add(transaction);
                        break;
                    case 10:
                        _bigDictionaryTransaction11[id].Add(transaction);
                        break;
                    case 11:
                        _bigDictionaryTransaction12[id].Add(transaction);
                        break;
                    case 12:
                        _bigDictionaryTransaction13[id].Add(transaction);
                        break;
                    case 13:
                        _bigDictionaryTransaction14[id].Add(transaction);
                        break;
                    case 14:
                        _bigDictionaryTransaction15[id].Add(transaction);
                        break;
                    case 15:
                        _bigDictionaryTransaction16[id].Add(transaction);
                        break;
                    case 16:
                        _bigDictionaryTransaction17[id].Add(transaction);
                        break;
                    case 17:
                        _bigDictionaryTransaction18[id].Add(transaction);
                        break;
                    case 18:
                        _bigDictionaryTransaction19[id].Add(transaction);
                        break;
                    case 19:
                        _bigDictionaryTransaction20[id].Add(transaction);
                        break;
                    case 20:
                        _bigDictionaryTransaction21[id].Add(transaction);
                        break;
                    case 21:
                        _bigDictionaryTransaction22[id].Add(transaction);
                        break;
                    case 22:
                        _bigDictionaryTransaction23[id].Add(transaction);
                        break;
                    case 23:
                        _bigDictionaryTransaction24[id].Add(transaction);
                        break;
                    case 24:
                        _bigDictionaryTransaction25[id].Add(transaction);
                        break;
                    case 25:
                        _bigDictionaryTransaction26[id].Add(transaction);
                        break;
                    case 26:
                        _bigDictionaryTransaction27[id].Add(transaction);
                        break;
                    case 27:
                        _bigDictionaryTransaction28[id].Add(transaction);
                        break;
                    case 28:
                        _bigDictionaryTransaction29[id].Add(transaction);
                        break;
                    case 29:
                        _bigDictionaryTransaction30[id].Add(transaction);
                        break;
                    case 30:
                        _bigDictionaryTransaction31[id].Add(transaction);
                        break;
                    case 31:
                        _bigDictionaryTransaction32[id].Add(transaction);
                        break;
                    case 32:
                        _bigDictionaryTransaction33[id].Add(transaction);
                        break;
                    case 33:
                        _bigDictionaryTransaction34[id].Add(transaction);
                        break;
                    case 34:
                        _bigDictionaryTransaction35[id].Add(transaction);
                        break;
                    case 35:
                        _bigDictionaryTransaction36[id].Add(transaction);
                        break;
                    case 36:
                        _bigDictionaryTransaction37[id].Add(transaction);
                        break;
                    case 37:
                        _bigDictionaryTransaction38[id].Add(transaction);
                        break;
                    case 38:
                        _bigDictionaryTransaction39[id].Add(transaction);
                        break;
                    case 39:
                        _bigDictionaryTransaction40[id].Add(transaction);
                        break;
                    case 40:
                        _bigDictionaryTransaction41[id].Add(transaction);
                        break;
                    case 41:
                        _bigDictionaryTransaction42[id].Add(transaction);
                        break;
                    case 42:
                        _bigDictionaryTransaction43[id].Add(transaction);
                        break;
                    case 43:
                        _bigDictionaryTransaction44[id].Add(transaction);
                        break;
                    case 44:
                        _bigDictionaryTransaction45[id].Add(transaction);
                        break;
                    case 45:
                        _bigDictionaryTransaction46[id].Add(transaction);
                        break;
                    case 46:
                        _bigDictionaryTransaction47[id].Add(transaction);
                        break;
                    case 47:
                        _bigDictionaryTransaction48[id].Add(transaction);
                        break;
                    case 48:
                        _bigDictionaryTransaction49[id].Add(transaction);
                        break;
                    case 49:
                        _bigDictionaryTransaction50[id].Add(transaction);
                        break;
                    case 50:
                        _bigDictionaryTransaction51[id].Add(transaction);
                        break;
                    case 51:
                        _bigDictionaryTransaction52[id].Add(transaction);
                        break;
                    case 52:
                        _bigDictionaryTransaction53[id].Add(transaction);
                        break;
                    case 53:
                        _bigDictionaryTransaction54[id].Add(transaction);
                        break;
                    case 54:
                        _bigDictionaryTransaction55[id].Add(transaction);
                        break;
                    case 55:
                        _bigDictionaryTransaction56[id].Add(transaction);
                        break;
                    case 56:
                        _bigDictionaryTransaction57[id].Add(transaction);
                        break;
                    case 57:
                        _bigDictionaryTransaction58[id].Add(transaction);
                        break;
                    case 58:
                        _bigDictionaryTransaction59[id].Add(transaction);
                        break;
                    case 59:
                        _bigDictionaryTransaction60[id].Add(transaction);
                        break;
                    case 60:
                        _bigDictionaryTransaction61[id].Add(transaction);
                        break;
                    case 61:
                        _bigDictionaryTransaction62[id].Add(transaction);
                        break;
                    case 62:
                        _bigDictionaryTransaction63[id].Add(transaction);
                        break;
                    case 63:
                        _bigDictionaryTransaction64[id].Add(transaction);
                        break;
                    case 64:
                        _bigDictionaryTransaction65[id].Add(transaction);
                        break;
                    case 65:
                        _bigDictionaryTransaction66[id].Add(transaction);
                        break;
                    case 66:
                        _bigDictionaryTransaction67[id].Add(transaction);
                        break;
                    case 67:
                        _bigDictionaryTransaction68[id].Add(transaction);
                        break;
                    case 68:
                        _bigDictionaryTransaction69[id].Add(transaction);
                        break;
                    case 69:
                        _bigDictionaryTransaction70[id].Add(transaction);
                        break;
                    case 70:
                        _bigDictionaryTransaction71[id].Add(transaction);
                        break;
                    case 71:
                        _bigDictionaryTransaction72[id].Add(transaction);
                        break;
                    case 72:
                        _bigDictionaryTransaction73[id].Add(transaction);
                        break;
                    case 73:
                        _bigDictionaryTransaction74[id].Add(transaction);
                        break;
                    case 74:
                        _bigDictionaryTransaction75[id].Add(transaction);
                        break;
                    case 75:
                        _bigDictionaryTransaction76[id].Add(transaction);
                        break;
                    case 76:
                        _bigDictionaryTransaction77[id].Add(transaction);
                        break;
                    case 77:
                        _bigDictionaryTransaction78[id].Add(transaction);
                        break;
                    case 78:
                        _bigDictionaryTransaction79[id].Add(transaction);
                        break;
                    case 79:
                        _bigDictionaryTransaction80[id].Add(transaction);
                        break;
                    case 80:
                        _bigDictionaryTransaction81[id].Add(transaction);
                        break;
                    case 81:
                        _bigDictionaryTransaction82[id].Add(transaction);
                        break;
                    case 82:
                        _bigDictionaryTransaction83[id].Add(transaction);
                        break;
                    case 83:
                        _bigDictionaryTransaction84[id].Add(transaction);
                        break;
                    case 84:
                        _bigDictionaryTransaction85[id].Add(transaction);
                        break;
                    case 85:
                        _bigDictionaryTransaction86[id].Add(transaction);
                        break;
                    case 86:
                        _bigDictionaryTransaction87[id].Add(transaction);
                        break;
                    case 87:
                        _bigDictionaryTransaction88[id].Add(transaction);
                        break;
                    case 88:
                        _bigDictionaryTransaction89[id].Add(transaction);
                        break;
                    case 89:
                        _bigDictionaryTransaction90[id].Add(transaction);
                        break;
                    case 90:
                        _bigDictionaryTransaction91[id].Add(transaction);
                        break;
                    case 91:
                        _bigDictionaryTransaction92[id].Add(transaction);
                        break;
                    case 92:
                        _bigDictionaryTransaction93[id].Add(transaction);
                        break;
                    case 93:
                        _bigDictionaryTransaction94[id].Add(transaction);
                        break;
                    case 94:
                        _bigDictionaryTransaction95[id].Add(transaction);
                        break;
                    case 95:
                        _bigDictionaryTransaction96[id].Add(transaction);
                        break;
                    case 96:
                        _bigDictionaryTransaction97[id].Add(transaction);
                        break;
                    case 97:
                        _bigDictionaryTransaction98[id].Add(transaction);
                        break;
                    case 98:
                        _bigDictionaryTransaction99[id].Add(transaction);
                        break;
                    case 99:
                        _bigDictionaryTransaction100[id].Add(transaction);
                        break;
                }
            }
            else
            {
                idDictionary = (long)(Math.Ceiling((double)Count / MaxTransactionPerDictionary));
                if (idDictionary > 0)
                {
                    idDictionary -= 1;
                }
                switch (idDictionary)
                {
                    case 0:
                        _bigDictionaryTransaction1.Add(id, new List<string>() { transaction });
                        break;
                    case 1:
                        _bigDictionaryTransaction2.Add(id, new List<string>() { transaction });
                        break;
                    case 2:
                        _bigDictionaryTransaction3.Add(id, new List<string>() { transaction });
                        break;
                    case 3:
                        _bigDictionaryTransaction4.Add(id, new List<string>() { transaction });
                        break;
                    case 4:
                        _bigDictionaryTransaction5.Add(id, new List<string>() { transaction });
                        break;
                    case 5:
                        _bigDictionaryTransaction6.Add(id, new List<string>() { transaction });
                        break;
                    case 6:
                        _bigDictionaryTransaction7.Add(id, new List<string>() { transaction });
                        break;
                    case 7:
                        _bigDictionaryTransaction8.Add(id, new List<string>() { transaction });
                        break;
                    case 8:
                        _bigDictionaryTransaction9.Add(id, new List<string>() { transaction });
                        break;
                    case 9:
                        _bigDictionaryTransaction10.Add(id, new List<string>() { transaction });
                        break;
                    case 10:
                        _bigDictionaryTransaction11.Add(id, new List<string>() { transaction });
                        break;
                    case 11:
                        _bigDictionaryTransaction12.Add(id, new List<string>() { transaction });
                        break;
                    case 12:
                        _bigDictionaryTransaction13.Add(id, new List<string>() { transaction });
                        break;
                    case 13:
                        _bigDictionaryTransaction14.Add(id, new List<string>() { transaction });
                        break;
                    case 14:
                        _bigDictionaryTransaction15.Add(id, new List<string>() { transaction });
                        break;
                    case 15:
                        _bigDictionaryTransaction16.Add(id, new List<string>() { transaction });
                        break;
                    case 16:
                        _bigDictionaryTransaction17.Add(id, new List<string>() { transaction });
                        break;
                    case 17:
                        _bigDictionaryTransaction18.Add(id, new List<string>() { transaction });
                        break;
                    case 18:
                        _bigDictionaryTransaction19.Add(id, new List<string>() { transaction });
                        break;
                    case 19:
                        _bigDictionaryTransaction20.Add(id, new List<string>() { transaction });
                        break;
                    case 20:
                        _bigDictionaryTransaction21.Add(id, new List<string>() { transaction });
                        break;
                    case 21:
                        _bigDictionaryTransaction22.Add(id, new List<string>() { transaction });
                        break;
                    case 22:
                        _bigDictionaryTransaction23.Add(id, new List<string>() { transaction });
                        break;
                    case 23:
                        _bigDictionaryTransaction24.Add(id, new List<string>() { transaction });
                        break;
                    case 24:
                        _bigDictionaryTransaction25.Add(id, new List<string>() { transaction });
                        break;
                    case 25:
                        _bigDictionaryTransaction26.Add(id, new List<string>() { transaction });
                        break;
                    case 26:
                        _bigDictionaryTransaction27.Add(id, new List<string>() { transaction });
                        break;
                    case 27:
                        _bigDictionaryTransaction28.Add(id, new List<string>() { transaction });
                        break;
                    case 28:
                        _bigDictionaryTransaction29.Add(id, new List<string>() { transaction });
                        break;
                    case 29:
                        _bigDictionaryTransaction30.Add(id, new List<string>() { transaction });
                        break;
                    case 30:
                        _bigDictionaryTransaction31.Add(id, new List<string>() { transaction });
                        break;
                    case 31:
                        _bigDictionaryTransaction32.Add(id, new List<string>() { transaction });
                        break;
                    case 32:
                        _bigDictionaryTransaction33.Add(id, new List<string>() { transaction });
                        break;
                    case 33:
                        _bigDictionaryTransaction34.Add(id, new List<string>() { transaction });
                        break;
                    case 34:
                        _bigDictionaryTransaction35.Add(id, new List<string>() { transaction });
                        break;
                    case 35:
                        _bigDictionaryTransaction36.Add(id, new List<string>() { transaction });
                        break;
                    case 36:
                        _bigDictionaryTransaction37.Add(id, new List<string>() { transaction });
                        break;
                    case 37:
                        _bigDictionaryTransaction38.Add(id, new List<string>() { transaction });
                        break;
                    case 38:
                        _bigDictionaryTransaction39.Add(id, new List<string>() { transaction });
                        break;
                    case 39:
                        _bigDictionaryTransaction40.Add(id, new List<string>() { transaction });
                        break;
                    case 40:
                        _bigDictionaryTransaction41.Add(id, new List<string>() { transaction });
                        break;
                    case 41:
                        _bigDictionaryTransaction42.Add(id, new List<string>() { transaction });
                        break;
                    case 42:
                        _bigDictionaryTransaction43.Add(id, new List<string>() { transaction });
                        break;
                    case 43:
                        _bigDictionaryTransaction44.Add(id, new List<string>() { transaction });
                        break;
                    case 44:
                        _bigDictionaryTransaction45.Add(id, new List<string>() { transaction });
                        break;
                    case 45:
                        _bigDictionaryTransaction46.Add(id, new List<string>() { transaction });
                        break;
                    case 46:
                        _bigDictionaryTransaction47.Add(id, new List<string>() { transaction });
                        break;
                    case 47:
                        _bigDictionaryTransaction48.Add(id, new List<string>() { transaction });
                        break;
                    case 48:
                        _bigDictionaryTransaction49.Add(id, new List<string>() { transaction });
                        break;
                    case 49:
                        _bigDictionaryTransaction50.Add(id, new List<string>() { transaction });
                        break;
                    case 50:
                        _bigDictionaryTransaction51.Add(id, new List<string>() { transaction });
                        break;
                    case 51:
                        _bigDictionaryTransaction52.Add(id, new List<string>() { transaction });
                        break;
                    case 52:
                        _bigDictionaryTransaction53.Add(id, new List<string>() { transaction });
                        break;
                    case 53:
                        _bigDictionaryTransaction54.Add(id, new List<string>() { transaction });
                        break;
                    case 54:
                        _bigDictionaryTransaction55.Add(id, new List<string>() { transaction });
                        break;
                    case 55:
                        _bigDictionaryTransaction56.Add(id, new List<string>() { transaction });
                        break;
                    case 56:
                        _bigDictionaryTransaction57.Add(id, new List<string>() { transaction });
                        break;
                    case 57:
                        _bigDictionaryTransaction58.Add(id, new List<string>() { transaction });
                        break;
                    case 58:
                        _bigDictionaryTransaction59.Add(id, new List<string>() { transaction });
                        break;
                    case 59:
                        _bigDictionaryTransaction60.Add(id, new List<string>() { transaction });
                        break;
                    case 60:
                        _bigDictionaryTransaction61.Add(id, new List<string>() { transaction });
                        break;
                    case 61:
                        _bigDictionaryTransaction62.Add(id, new List<string>() { transaction });
                        break;
                    case 62:
                        _bigDictionaryTransaction63.Add(id, new List<string>() { transaction });
                        break;
                    case 63:
                        _bigDictionaryTransaction64.Add(id, new List<string>() { transaction });
                        break;
                    case 64:
                        _bigDictionaryTransaction65.Add(id, new List<string>() { transaction });
                        break;
                    case 65:
                        _bigDictionaryTransaction66.Add(id, new List<string>() { transaction });
                        break;
                    case 66:
                        _bigDictionaryTransaction67.Add(id, new List<string>() { transaction });
                        break;
                    case 67:
                        _bigDictionaryTransaction68.Add(id, new List<string>() { transaction });
                        break;
                    case 68:
                        _bigDictionaryTransaction69.Add(id, new List<string>() { transaction });
                        break;
                    case 69:
                        _bigDictionaryTransaction70.Add(id, new List<string>() { transaction });
                        break;
                    case 70:
                        _bigDictionaryTransaction71.Add(id, new List<string>() { transaction });
                        break;
                    case 71:
                        _bigDictionaryTransaction72.Add(id, new List<string>() { transaction });
                        break;
                    case 72:
                        _bigDictionaryTransaction73.Add(id, new List<string>() { transaction });
                        break;
                    case 73:
                        _bigDictionaryTransaction74.Add(id, new List<string>() { transaction });
                        break;
                    case 74:
                        _bigDictionaryTransaction75.Add(id, new List<string>() { transaction });
                        break;
                    case 75:
                        _bigDictionaryTransaction76.Add(id, new List<string>() { transaction });
                        break;
                    case 76:
                        _bigDictionaryTransaction77.Add(id, new List<string>() { transaction });
                        break;
                    case 77:
                        _bigDictionaryTransaction78.Add(id, new List<string>() { transaction });
                        break;
                    case 78:
                        _bigDictionaryTransaction79.Add(id, new List<string>() { transaction });
                        break;
                    case 79:
                        _bigDictionaryTransaction80.Add(id, new List<string>() { transaction });
                        break;
                    case 80:
                        _bigDictionaryTransaction81.Add(id, new List<string>() { transaction });
                        break;
                    case 81:
                        _bigDictionaryTransaction82.Add(id, new List<string>() { transaction });
                        break;
                    case 82:
                        _bigDictionaryTransaction83.Add(id, new List<string>() { transaction });
                        break;
                    case 83:
                        _bigDictionaryTransaction84.Add(id, new List<string>() { transaction });
                        break;
                    case 84:
                        _bigDictionaryTransaction85.Add(id, new List<string>() { transaction });
                        break;
                    case 85:
                        _bigDictionaryTransaction86.Add(id, new List<string>() { transaction });
                        break;
                    case 86:
                        _bigDictionaryTransaction87.Add(id, new List<string>() { transaction });
                        break;
                    case 87:
                        _bigDictionaryTransaction88.Add(id, new List<string>() { transaction });
                        break;
                    case 88:
                        _bigDictionaryTransaction89.Add(id, new List<string>() { transaction });
                        break;
                    case 89:
                        _bigDictionaryTransaction90.Add(id, new List<string>() { transaction });
                        break;
                    case 90:
                        _bigDictionaryTransaction91.Add(id, new List<string>() { transaction });
                        break;
                    case 91:
                        _bigDictionaryTransaction92.Add(id, new List<string>() { transaction });
                        break;
                    case 92:
                        _bigDictionaryTransaction93.Add(id, new List<string>() { transaction });
                        break;
                    case 93:
                        _bigDictionaryTransaction94.Add(id, new List<string>() { transaction });
                        break;
                    case 94:
                        _bigDictionaryTransaction95.Add(id, new List<string>() { transaction });
                        break;
                    case 95:
                        _bigDictionaryTransaction96.Add(id, new List<string>() { transaction });
                        break;
                    case 96:
                        _bigDictionaryTransaction97.Add(id, new List<string>() { transaction });
                        break;
                    case 97:
                        _bigDictionaryTransaction98.Add(id, new List<string>() { transaction });
                        break;
                    case 98:
                        _bigDictionaryTransaction99.Add(id, new List<string>() { transaction });
                        break;
                    case 99:
                        _bigDictionaryTransaction100.Add(id, new List<string>() { transaction });
                        break;
                }
            }
        }

        /// <summary>
        /// Retrieve total transaction from wallet.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetTransactionCount(float id)
        {

            long idDictionary = ContainsKey(id);
            if (idDictionary != -1)
            {
                switch (idDictionary)
                {
                    case 0:
                        return _bigDictionaryTransaction1[id].Count;
                    case 1:
                        return _bigDictionaryTransaction2[id].Count;
                    case 2:
                        return _bigDictionaryTransaction3[id].Count;
                    case 3:
                        return _bigDictionaryTransaction4[id].Count;
                    case 4:
                        return _bigDictionaryTransaction5[id].Count;
                    case 5:
                        return _bigDictionaryTransaction6[id].Count;
                    case 6:
                        return _bigDictionaryTransaction7[id].Count;
                    case 7:
                        return _bigDictionaryTransaction8[id].Count;
                    case 8:
                        return _bigDictionaryTransaction9[id].Count;
                    case 9:
                        return _bigDictionaryTransaction10[id].Count;
                    case 10:
                        return _bigDictionaryTransaction11[id].Count;
                    case 11:
                        return _bigDictionaryTransaction12[id].Count;
                    case 12:
                        return _bigDictionaryTransaction13[id].Count;
                    case 13:
                        return _bigDictionaryTransaction14[id].Count;
                    case 14:
                        return _bigDictionaryTransaction15[id].Count;
                    case 15:
                        return _bigDictionaryTransaction16[id].Count;
                    case 16:
                        return _bigDictionaryTransaction17[id].Count;
                    case 17:
                        return _bigDictionaryTransaction18[id].Count;
                    case 18:
                        return _bigDictionaryTransaction19[id].Count;
                    case 19:
                        return _bigDictionaryTransaction20[id].Count;
                    case 20:
                        return _bigDictionaryTransaction21[id].Count;
                    case 21:
                        return _bigDictionaryTransaction22[id].Count;
                    case 22:
                        return _bigDictionaryTransaction23[id].Count;
                    case 23:
                        return _bigDictionaryTransaction24[id].Count;
                    case 24:
                        return _bigDictionaryTransaction25[id].Count;
                    case 25:
                        return _bigDictionaryTransaction26[id].Count;
                    case 26:
                        return _bigDictionaryTransaction27[id].Count;
                    case 27:
                        return _bigDictionaryTransaction28[id].Count;
                    case 28:
                        return _bigDictionaryTransaction29[id].Count;
                    case 29:
                        return _bigDictionaryTransaction30[id].Count;
                    case 30:
                        return _bigDictionaryTransaction31[id].Count;
                    case 31:
                        return _bigDictionaryTransaction32[id].Count;
                    case 32:
                        return _bigDictionaryTransaction33[id].Count;
                    case 33:
                        return _bigDictionaryTransaction34[id].Count;
                    case 34:
                        return _bigDictionaryTransaction35[id].Count;
                    case 35:
                        return _bigDictionaryTransaction36[id].Count;
                    case 36:
                        return _bigDictionaryTransaction37[id].Count;
                    case 37:
                        return _bigDictionaryTransaction38[id].Count;
                    case 38:
                        return _bigDictionaryTransaction39[id].Count;
                    case 39:
                        return _bigDictionaryTransaction40[id].Count;
                    case 40:
                        return _bigDictionaryTransaction41[id].Count;
                    case 41:
                        return _bigDictionaryTransaction42[id].Count;
                    case 42:
                        return _bigDictionaryTransaction43[id].Count;
                    case 43:
                        return _bigDictionaryTransaction44[id].Count;
                    case 44:
                        return _bigDictionaryTransaction45[id].Count;
                    case 45:
                        return _bigDictionaryTransaction46[id].Count;
                    case 46:
                        return _bigDictionaryTransaction47[id].Count;
                    case 47:
                        return _bigDictionaryTransaction48[id].Count;
                    case 48:
                        return _bigDictionaryTransaction49[id].Count;
                    case 49:
                        return _bigDictionaryTransaction50[id].Count;
                    case 50:
                        return _bigDictionaryTransaction51[id].Count;
                    case 51:
                        return _bigDictionaryTransaction52[id].Count;
                    case 52:
                        return _bigDictionaryTransaction53[id].Count;
                    case 53:
                        return _bigDictionaryTransaction54[id].Count;
                    case 54:
                        return _bigDictionaryTransaction55[id].Count;
                    case 55:
                        return _bigDictionaryTransaction56[id].Count;
                    case 56:
                        return _bigDictionaryTransaction57[id].Count;
                    case 57:
                        return _bigDictionaryTransaction58[id].Count;
                    case 58:
                        return _bigDictionaryTransaction59[id].Count;
                    case 59:
                        return _bigDictionaryTransaction60[id].Count;
                    case 60:
                        return _bigDictionaryTransaction61[id].Count;
                    case 61:
                        return _bigDictionaryTransaction62[id].Count;
                    case 62:
                        return _bigDictionaryTransaction63[id].Count;
                    case 63:
                        return _bigDictionaryTransaction64[id].Count;
                    case 64:
                        return _bigDictionaryTransaction65[id].Count;
                    case 65:
                        return _bigDictionaryTransaction66[id].Count;
                    case 66:
                        return _bigDictionaryTransaction67[id].Count;
                    case 67:
                        return _bigDictionaryTransaction68[id].Count;
                    case 68:
                        return _bigDictionaryTransaction69[id].Count;
                    case 69:
                        return _bigDictionaryTransaction70[id].Count;
                    case 70:
                        return _bigDictionaryTransaction71[id].Count;
                    case 71:
                        return _bigDictionaryTransaction72[id].Count;
                    case 72:
                        return _bigDictionaryTransaction73[id].Count;
                    case 73:
                        return _bigDictionaryTransaction74[id].Count;
                    case 74:
                        return _bigDictionaryTransaction75[id].Count;
                    case 75:
                        return _bigDictionaryTransaction76[id].Count;
                    case 76:
                        return _bigDictionaryTransaction77[id].Count;
                    case 77:
                        return _bigDictionaryTransaction78[id].Count;
                    case 78:
                        return _bigDictionaryTransaction79[id].Count;
                    case 79:
                        return _bigDictionaryTransaction80[id].Count;
                    case 80:
                        return _bigDictionaryTransaction81[id].Count;
                    case 81:
                        return _bigDictionaryTransaction82[id].Count;
                    case 82:
                        return _bigDictionaryTransaction83[id].Count;
                    case 83:
                        return _bigDictionaryTransaction84[id].Count;
                    case 84:
                        return _bigDictionaryTransaction85[id].Count;
                    case 85:
                        return _bigDictionaryTransaction86[id].Count;
                    case 86:
                        return _bigDictionaryTransaction87[id].Count;
                    case 87:
                        return _bigDictionaryTransaction88[id].Count;
                    case 88:
                        return _bigDictionaryTransaction89[id].Count;
                    case 89:
                        return _bigDictionaryTransaction90[id].Count;
                    case 90:
                        return _bigDictionaryTransaction91[id].Count;
                    case 91:
                        return _bigDictionaryTransaction92[id].Count;
                    case 92:
                        return _bigDictionaryTransaction93[id].Count;
                    case 93:
                        return _bigDictionaryTransaction94[id].Count;
                    case 94:
                        return _bigDictionaryTransaction95[id].Count;
                    case 95:
                        return _bigDictionaryTransaction96[id].Count;
                    case 96:
                        return _bigDictionaryTransaction97[id].Count;
                    case 97:
                        return _bigDictionaryTransaction98[id].Count;
                    case 98:
                        return _bigDictionaryTransaction99[id].Count;
                    case 99:
                        return _bigDictionaryTransaction100[id].Count;
                }
            }
            return 0;
        }

        /// <summary>
        /// Retrieve total transaction from wallet.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetTransactionPerId(float id, int transactionId)
        {

            long idDictionary = ContainsKey(id);
            if (idDictionary != -1)
            {
                switch (idDictionary)
                {
                    case 0:
                        return _bigDictionaryTransaction1[id][transactionId];
                    case 1:
                        return _bigDictionaryTransaction2[id][transactionId];
                    case 2:
                        return _bigDictionaryTransaction3[id][transactionId];
                    case 3:
                        return _bigDictionaryTransaction4[id][transactionId];
                    case 4:
                        return _bigDictionaryTransaction5[id][transactionId];
                    case 5:
                        return _bigDictionaryTransaction6[id][transactionId];
                    case 6:
                        return _bigDictionaryTransaction7[id][transactionId];
                    case 7:
                        return _bigDictionaryTransaction8[id][transactionId];
                    case 8:
                        return _bigDictionaryTransaction9[id][transactionId];
                    case 9:
                        return _bigDictionaryTransaction10[id][transactionId];
                    case 10:
                        return _bigDictionaryTransaction11[id][transactionId];
                    case 11:
                        return _bigDictionaryTransaction12[id][transactionId];
                    case 12:
                        return _bigDictionaryTransaction13[id][transactionId];
                    case 13:
                        return _bigDictionaryTransaction14[id][transactionId];
                    case 14:
                        return _bigDictionaryTransaction15[id][transactionId];
                    case 15:
                        return _bigDictionaryTransaction16[id][transactionId];
                    case 16:
                        return _bigDictionaryTransaction17[id][transactionId];
                    case 17:
                        return _bigDictionaryTransaction18[id][transactionId];
                    case 18:
                        return _bigDictionaryTransaction19[id][transactionId];
                    case 19:
                        return _bigDictionaryTransaction20[id][transactionId];
                    case 20:
                        return _bigDictionaryTransaction21[id][transactionId];
                    case 21:
                        return _bigDictionaryTransaction22[id][transactionId];
                    case 22:
                        return _bigDictionaryTransaction23[id][transactionId];
                    case 23:
                        return _bigDictionaryTransaction24[id][transactionId];
                    case 24:
                        return _bigDictionaryTransaction25[id][transactionId];
                    case 25:
                        return _bigDictionaryTransaction26[id][transactionId];
                    case 26:
                        return _bigDictionaryTransaction27[id][transactionId];
                    case 27:
                        return _bigDictionaryTransaction28[id][transactionId];
                    case 28:
                        return _bigDictionaryTransaction29[id][transactionId];
                    case 29:
                        return _bigDictionaryTransaction30[id][transactionId];
                    case 30:
                        return _bigDictionaryTransaction31[id][transactionId];
                    case 31:
                        return _bigDictionaryTransaction32[id][transactionId];
                    case 32:
                        return _bigDictionaryTransaction33[id][transactionId];
                    case 33:
                        return _bigDictionaryTransaction34[id][transactionId];
                    case 34:
                        return _bigDictionaryTransaction35[id][transactionId];
                    case 35:
                        return _bigDictionaryTransaction36[id][transactionId];
                    case 36:
                        return _bigDictionaryTransaction37[id][transactionId];
                    case 37:
                        return _bigDictionaryTransaction38[id][transactionId];
                    case 38:
                        return _bigDictionaryTransaction39[id][transactionId];
                    case 39:
                        return _bigDictionaryTransaction40[id][transactionId];
                    case 40:
                        return _bigDictionaryTransaction41[id][transactionId];
                    case 41:
                        return _bigDictionaryTransaction42[id][transactionId];
                    case 42:
                        return _bigDictionaryTransaction43[id][transactionId];
                    case 43:
                        return _bigDictionaryTransaction44[id][transactionId];
                    case 44:
                        return _bigDictionaryTransaction45[id][transactionId];
                    case 45:
                        return _bigDictionaryTransaction46[id][transactionId];
                    case 46:
                        return _bigDictionaryTransaction47[id][transactionId];
                    case 47:
                        return _bigDictionaryTransaction48[id][transactionId];
                    case 48:
                        return _bigDictionaryTransaction49[id][transactionId];
                    case 49:
                        return _bigDictionaryTransaction50[id][transactionId];
                    case 50:
                        return _bigDictionaryTransaction51[id][transactionId];
                    case 51:
                        return _bigDictionaryTransaction52[id][transactionId];
                    case 52:
                        return _bigDictionaryTransaction53[id][transactionId];
                    case 53:
                        return _bigDictionaryTransaction54[id][transactionId];
                    case 54:
                        return _bigDictionaryTransaction55[id][transactionId];
                    case 55:
                        return _bigDictionaryTransaction56[id][transactionId];
                    case 56:
                        return _bigDictionaryTransaction57[id][transactionId];
                    case 57:
                        return _bigDictionaryTransaction58[id][transactionId];
                    case 58:
                        return _bigDictionaryTransaction59[id][transactionId];
                    case 59:
                        return _bigDictionaryTransaction60[id][transactionId];
                    case 60:
                        return _bigDictionaryTransaction61[id][transactionId];
                    case 61:
                        return _bigDictionaryTransaction62[id][transactionId];
                    case 62:
                        return _bigDictionaryTransaction63[id][transactionId];
                    case 63:
                        return _bigDictionaryTransaction64[id][transactionId];
                    case 64:
                        return _bigDictionaryTransaction65[id][transactionId];
                    case 65:
                        return _bigDictionaryTransaction66[id][transactionId];
                    case 66:
                        return _bigDictionaryTransaction67[id][transactionId];
                    case 67:
                        return _bigDictionaryTransaction68[id][transactionId];
                    case 68:
                        return _bigDictionaryTransaction69[id][transactionId];
                    case 69:
                        return _bigDictionaryTransaction70[id][transactionId];
                    case 70:
                        return _bigDictionaryTransaction71[id][transactionId];
                    case 71:
                        return _bigDictionaryTransaction72[id][transactionId];
                    case 72:
                        return _bigDictionaryTransaction73[id][transactionId];
                    case 73:
                        return _bigDictionaryTransaction74[id][transactionId];
                    case 74:
                        return _bigDictionaryTransaction75[id][transactionId];
                    case 75:
                        return _bigDictionaryTransaction76[id][transactionId];
                    case 76:
                        return _bigDictionaryTransaction77[id][transactionId];
                    case 77:
                        return _bigDictionaryTransaction78[id][transactionId];
                    case 78:
                        return _bigDictionaryTransaction79[id][transactionId];
                    case 79:
                        return _bigDictionaryTransaction80[id][transactionId];
                    case 80:
                        return _bigDictionaryTransaction81[id][transactionId];
                    case 81:
                        return _bigDictionaryTransaction82[id][transactionId];
                    case 82:
                        return _bigDictionaryTransaction83[id][transactionId];
                    case 83:
                        return _bigDictionaryTransaction84[id][transactionId];
                    case 84:
                        return _bigDictionaryTransaction85[id][transactionId];
                    case 85:
                        return _bigDictionaryTransaction86[id][transactionId];
                    case 86:
                        return _bigDictionaryTransaction87[id][transactionId];
                    case 87:
                        return _bigDictionaryTransaction88[id][transactionId];
                    case 88:
                        return _bigDictionaryTransaction89[id][transactionId];
                    case 89:
                        return _bigDictionaryTransaction90[id][transactionId];
                    case 90:
                        return _bigDictionaryTransaction91[id][transactionId];
                    case 91:
                        return _bigDictionaryTransaction92[id][transactionId];
                    case 92:
                        return _bigDictionaryTransaction93[id][transactionId];
                    case 93:
                        return _bigDictionaryTransaction94[id][transactionId];
                    case 94:
                        return _bigDictionaryTransaction95[id][transactionId];
                    case 95:
                        return _bigDictionaryTransaction96[id][transactionId];
                    case 96:
                        return _bigDictionaryTransaction97[id][transactionId];
                    case 97:
                        return _bigDictionaryTransaction98[id][transactionId];
                    case 98:
                        return _bigDictionaryTransaction99[id][transactionId];
                    case 99:
                        return _bigDictionaryTransaction100[id][transactionId];
                }
            }
            return "WRONG";
        }

        /// <summary>
        /// Retrieve total transaction saved.
        /// </summary>
        /// <returns></returns>
        public long CountTransaction()
        {
            long countTransaction = 0;

            foreach (var transactionSortedEntry in _bigDictionaryTransaction1)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction2)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction3)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction4)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction5)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction6)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction7)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction8)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction9)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction10)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction11)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction12)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction13)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction14)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction15)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction16)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction17)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction18)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction19)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction20)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction21)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction22)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction23)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction24)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction25)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction26)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction27)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction28)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction29)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction30)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction31)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction31)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction32)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction33)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction34)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction35)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction36)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction37)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction38)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction39)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction40)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction41)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction42)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction43)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction44)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction45)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction46)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction47)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction48)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction49)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction50)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction51)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction52)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction53)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction54)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction55)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction56)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction57)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction58)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction59)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction60)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction61)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction62)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction63)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction64)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction65)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction66)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction67)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction68)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction69)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction70)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction71)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction72)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction73)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction74)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction75)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction76)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction77)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction78)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction79)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction80)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction81)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction82)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction83)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction84)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction85)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction86)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction87)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction88)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction89)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction90)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction91)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction92)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction93)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction94)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction95)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction96)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction97)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction98)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction99)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }
            foreach (var transactionSortedEntry in _bigDictionaryTransaction100)
            {
                countTransaction += transactionSortedEntry.Value.Count;
            }

            return countTransaction;
        }


        /// <summary>
        /// Check on every dictionary the id existance.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public long ContainsKey(float id)
        {
            if (_bigDictionaryTransaction1.ContainsKey(id))
            {
                return 0;
            }
            if (_bigDictionaryTransaction2.ContainsKey(id))
            {
                return 1;
            }
            if (_bigDictionaryTransaction3.ContainsKey(id))
            {
                return 2;
            }
            if (_bigDictionaryTransaction4.ContainsKey(id))
            {
                return 3;
            }
            if (_bigDictionaryTransaction5.ContainsKey(id))
            {
                return 4;
            }
            if (_bigDictionaryTransaction6.ContainsKey(id))
            {
                return 5;
            }
            if (_bigDictionaryTransaction7.ContainsKey(id))
            {
                return 6;
            }
            if (_bigDictionaryTransaction8.ContainsKey(id))
            {
                return 7;
            }
            if (_bigDictionaryTransaction9.ContainsKey(id))
            {
                return 8;
            }
            if (_bigDictionaryTransaction10.ContainsKey(id))
            {
                return 9;
            }
            if (_bigDictionaryTransaction11.ContainsKey(id))
            {
                return 10;
            }
            if (_bigDictionaryTransaction12.ContainsKey(id))
            {
                return 11;
            }
            if (_bigDictionaryTransaction13.ContainsKey(id))
            {
                return 12;
            }
            if (_bigDictionaryTransaction14.ContainsKey(id))
            {
                return 13;
            }
            if (_bigDictionaryTransaction15.ContainsKey(id))
            {
                return 14;
            }
            if (_bigDictionaryTransaction16.ContainsKey(id))
            {
                return 15;
            }
            if (_bigDictionaryTransaction17.ContainsKey(id))
            {
                return 16;
            }
            if (_bigDictionaryTransaction18.ContainsKey(id))
            {
                return 17;
            }
            if (_bigDictionaryTransaction19.ContainsKey(id))
            {
                return 18;
            }
            if (_bigDictionaryTransaction20.ContainsKey(id))
            {
                return 19;
            }
            if (_bigDictionaryTransaction21.ContainsKey(id))
            {
                return 20;
            }
            if (_bigDictionaryTransaction22.ContainsKey(id))
            {
                return 21;
            }
            if (_bigDictionaryTransaction23.ContainsKey(id))
            {
                return 22;
            }
            if (_bigDictionaryTransaction24.ContainsKey(id))
            {
                return 23;
            }
            if (_bigDictionaryTransaction25.ContainsKey(id))
            {
                return 24;
            }
            if (_bigDictionaryTransaction26.ContainsKey(id))
            {
                return 25;
            }
            if (_bigDictionaryTransaction27.ContainsKey(id))
            {
                return 26;
            }
            if (_bigDictionaryTransaction28.ContainsKey(id))
            {
                return 27;
            }
            if (_bigDictionaryTransaction29.ContainsKey(id))
            {
                return 28;
            }
            if (_bigDictionaryTransaction30.ContainsKey(id))
            {
                return 29;
            }
            if (_bigDictionaryTransaction31.ContainsKey(id))
            {
                return 30;
            }
            if (_bigDictionaryTransaction32.ContainsKey(id))
            {
                return 31;
            }
            if (_bigDictionaryTransaction33.ContainsKey(id))
            {
                return 32;
            }
            if (_bigDictionaryTransaction34.ContainsKey(id))
            {
                return 33;
            }
            if (_bigDictionaryTransaction35.ContainsKey(id))
            {
                return 34;
            }
            if (_bigDictionaryTransaction36.ContainsKey(id))
            {
                return 35;
            }
            if (_bigDictionaryTransaction37.ContainsKey(id))
            {
                return 36;
            }
            if (_bigDictionaryTransaction38.ContainsKey(id))
            {
                return 37;
            }
            if (_bigDictionaryTransaction39.ContainsKey(id))
            {
                return 38;
            }
            if (_bigDictionaryTransaction40.ContainsKey(id))
            {
                return 39;
            }
            if (_bigDictionaryTransaction41.ContainsKey(id))
            {
                return 40;
            }
            if (_bigDictionaryTransaction42.ContainsKey(id))
            {
                return 41;
            }
            if (_bigDictionaryTransaction43.ContainsKey(id))
            {
                return 42;
            }
            if (_bigDictionaryTransaction44.ContainsKey(id))
            {
                return 43;
            }
            if (_bigDictionaryTransaction45.ContainsKey(id))
            {
                return 44;
            }
            if (_bigDictionaryTransaction46.ContainsKey(id))
            {
                return 45;
            }
            if (_bigDictionaryTransaction47.ContainsKey(id))
            {
                return 46;
            }
            if (_bigDictionaryTransaction48.ContainsKey(id))
            {
                return 47;
            }
            if (_bigDictionaryTransaction49.ContainsKey(id))
            {
                return 48;
            }
            if (_bigDictionaryTransaction50.ContainsKey(id))
            {
                return 49;
            }
            if (_bigDictionaryTransaction51.ContainsKey(id))
            {
                return 50;
            }
            if (_bigDictionaryTransaction52.ContainsKey(id))
            {
                return 51;
            }
            if (_bigDictionaryTransaction53.ContainsKey(id))
            {
                return 52;
            }
            if (_bigDictionaryTransaction54.ContainsKey(id))
            {
                return 53;
            }
            if (_bigDictionaryTransaction55.ContainsKey(id))
            {
                return 54;
            }
            if (_bigDictionaryTransaction56.ContainsKey(id))
            {
                return 55;
            }
            if (_bigDictionaryTransaction57.ContainsKey(id))
            {
                return 56;
            }
            if (_bigDictionaryTransaction58.ContainsKey(id))
            {
                return 57;
            }
            if (_bigDictionaryTransaction59.ContainsKey(id))
            {
                return 58;
            }
            if (_bigDictionaryTransaction60.ContainsKey(id))
            {
                return 59;
            }
            if (_bigDictionaryTransaction61.ContainsKey(id))
            {
                return 60;
            }
            if (_bigDictionaryTransaction62.ContainsKey(id))
            {
                return 61;
            }
            if (_bigDictionaryTransaction63.ContainsKey(id))
            {
                return 62;
            }
            if (_bigDictionaryTransaction64.ContainsKey(id))
            {
                return 63;
            }
            if (_bigDictionaryTransaction65.ContainsKey(id))
            {
                return 64;
            }
            if (_bigDictionaryTransaction66.ContainsKey(id))
            {
                return 65;
            }
            if (_bigDictionaryTransaction67.ContainsKey(id))
            {
                return 66;
            }
            if (_bigDictionaryTransaction68.ContainsKey(id))
            {
                return 67;
            }
            if (_bigDictionaryTransaction69.ContainsKey(id))
            {
                return 68;
            }
            if (_bigDictionaryTransaction70.ContainsKey(id))
            {
                return 69;
            }
            if (_bigDictionaryTransaction71.ContainsKey(id))
            {
                return 70;
            }
            if (_bigDictionaryTransaction72.ContainsKey(id))
            {
                return 71;
            }
            if (_bigDictionaryTransaction73.ContainsKey(id))
            {
                return 72;
            }
            if (_bigDictionaryTransaction74.ContainsKey(id))
            {
                return 73;
            }
            if (_bigDictionaryTransaction75.ContainsKey(id))
            {
                return 74;
            }
            if (_bigDictionaryTransaction76.ContainsKey(id))
            {
                return 75;
            }
            if (_bigDictionaryTransaction77.ContainsKey(id))
            {
                return 76;
            }
            if (_bigDictionaryTransaction78.ContainsKey(id))
            {
                return 77;
            }
            if (_bigDictionaryTransaction79.ContainsKey(id))
            {
                return 78;
            }
            if (_bigDictionaryTransaction80.ContainsKey(id))
            {
                return 79;
            }
            if (_bigDictionaryTransaction81.ContainsKey(id))
            {
                return 80;
            }
            if (_bigDictionaryTransaction82.ContainsKey(id))
            {
                return 81;
            }
            if (_bigDictionaryTransaction83.ContainsKey(id))
            {
                return 82;
            }
            if (_bigDictionaryTransaction84.ContainsKey(id))
            {
                return 83;
            }
            if (_bigDictionaryTransaction85.ContainsKey(id))
            {
                return 84;
            }
            if (_bigDictionaryTransaction86.ContainsKey(id))
            {
                return 85;
            }
            if (_bigDictionaryTransaction87.ContainsKey(id))
            {
                return 86;
            }
            if (_bigDictionaryTransaction88.ContainsKey(id))
            {
                return 87;
            }
            if (_bigDictionaryTransaction89.ContainsKey(id))
            {
                return 88;
            }
            if (_bigDictionaryTransaction90.ContainsKey(id))
            {
                return 89;
            }
            if (_bigDictionaryTransaction91.ContainsKey(id))
            {
                return 90;
            }
            if (_bigDictionaryTransaction92.ContainsKey(id))
            {
                return 91;
            }
            if (_bigDictionaryTransaction93.ContainsKey(id))
            {
                return 92;
            }
            if (_bigDictionaryTransaction94.ContainsKey(id))
            {
                return 93;
            }
            if (_bigDictionaryTransaction95.ContainsKey(id))
            {
                return 94;
            }
            if (_bigDictionaryTransaction96.ContainsKey(id))
            {
                return 95;
            }
            if (_bigDictionaryTransaction97.ContainsKey(id))
            {
                return 96;
            }
            if (_bigDictionaryTransaction98.ContainsKey(id))
            {
                return 97;
            }
            if (_bigDictionaryTransaction99.ContainsKey(id))
            {
                return 98;
            }
            if (_bigDictionaryTransaction100.ContainsKey(id))
            {
                return 99;
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
