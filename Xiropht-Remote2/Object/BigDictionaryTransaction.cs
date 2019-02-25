using System.Collections.Generic;

namespace Xiropht_RemoteNode.Object
{
    public class BigDictionaryTransaction
    {
        private Dictionary<long, string> _bigDictionaryTransaction1; // Range: 0 - 999999999
        private Dictionary<long, string> _bigDictionaryTransaction2; // Range: 999999999 - 1999999999
        private Dictionary<long, string> _bigDictionaryTransaction3; // Range: 1999999999 - 2999999999
        private Dictionary<long, string> _bigDictionaryTransaction4; // Range: 2999999999 - 3999999999
        private Dictionary<long, string> _bigDictionaryTransaction5; // Range: 3999999999 - 4999999999
        private Dictionary<long, string> _bigDictionaryTransaction6; // Range: 4999999999 - 5999999999
        private Dictionary<long, string> _bigDictionaryTransaction7; // Range: 5999999999 - 6999999999
        private Dictionary<long, string> _bigDictionaryTransaction8; // Range: 7999999999 - 8999999999
        private Dictionary<long, string> _bigDictionaryTransaction9; // Range: 8999999999 - 9999999999
        private Dictionary<long, string> _bigDictionaryTransaction10; // Range: 9999999999 - 1099999999

        private const int MaxTransactionPerDictionary = 1000000000; // 1 billions of transaction per dictionary

        /// <summary>
        /// Constructor
        /// </summary>
        public BigDictionaryTransaction()
        {
            _bigDictionaryTransaction1 = new Dictionary<long, string>();
            _bigDictionaryTransaction2 = new Dictionary<long, string>();
            _bigDictionaryTransaction3 = new Dictionary<long, string>();
            _bigDictionaryTransaction4 = new Dictionary<long, string>();
            _bigDictionaryTransaction5 = new Dictionary<long, string>();
            _bigDictionaryTransaction6 = new Dictionary<long, string>();
            _bigDictionaryTransaction7 = new Dictionary<long, string>();
            _bigDictionaryTransaction8 = new Dictionary<long, string>();
            _bigDictionaryTransaction9 = new Dictionary<long, string>();
            _bigDictionaryTransaction10 = new Dictionary<long, string>();
        }

        /// <summary>
        /// Insert transaction
        /// </summary>
        /// <param name="id"></param>
        /// <param name="transaction"></param>
        public void InsertTransaction(long id, string transaction)
        {
            long idDictionary = id / MaxTransactionPerDictionary;
            switch (idDictionary)
            {
                case 0:
                    _bigDictionaryTransaction1.Add(id, transaction);
                    break;
                case 1:
                    _bigDictionaryTransaction2.Add(id, transaction);
                    break;
                case 2:
                    _bigDictionaryTransaction3.Add(id, transaction);
                    break;
                case 3:
                    _bigDictionaryTransaction4.Add(id, transaction);
                    break;
                case 4:
                    _bigDictionaryTransaction5.Add(id, transaction);
                    break;
                case 5:
                    _bigDictionaryTransaction6.Add(id, transaction);
                    break;
                case 6:
                    _bigDictionaryTransaction7.Add(id, transaction);
                    break;
                case 7:
                    _bigDictionaryTransaction8.Add(id, transaction);
                    break;
                case 8:
                    _bigDictionaryTransaction9.Add(id, transaction);
                    break;
                case 9:
                    _bigDictionaryTransaction10.Add(id, transaction);
                    break;
            }
        }



        /// <summary>
        /// Retrieve transaction information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetTransaction(long id)
        {
            if (id < 0)
            {
                return "WRONG";
            }

            long idDictionary = id / MaxTransactionPerDictionary;
            switch (idDictionary)
            {
                case 0:
                    return _bigDictionaryTransaction1[id];
                case 1:
                    return _bigDictionaryTransaction2[id];
                case 2:
                    return _bigDictionaryTransaction3[id];
                case 3:
                    return _bigDictionaryTransaction4[id];
                case 4:
                    return _bigDictionaryTransaction5[id];
                case 5:
                    return _bigDictionaryTransaction6[id];
                case 6:
                    return _bigDictionaryTransaction7[id];
                case 7:
                    return _bigDictionaryTransaction8[id];
                case 8:
                    return _bigDictionaryTransaction9[id];
                case 9:
                    return _bigDictionaryTransaction10[id];
            }
            return "WRONG";
        }

        /// <summary>
        /// Retrieve total transaction saved.
        /// </summary>
        /// <returns></returns>
        public long Count()
        {
            return _bigDictionaryTransaction1.Count + _bigDictionaryTransaction2.Count + _bigDictionaryTransaction3.Count + _bigDictionaryTransaction4.Count + _bigDictionaryTransaction5.Count + _bigDictionaryTransaction6.Count + _bigDictionaryTransaction7.Count + _bigDictionaryTransaction8.Count + _bigDictionaryTransaction9.Count + _bigDictionaryTransaction10.Count;
        }

        /// <summary>
        /// Check on every dictionary the value existance.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ContainsValue(string value)
        {
            if (_bigDictionaryTransaction1.ContainsValue(value))
            {
                return true;
            }
            if (_bigDictionaryTransaction2.ContainsValue(value))
            {
                return true;
            }
            if (_bigDictionaryTransaction3.ContainsValue(value))
            {
                return true;
            }
            if (_bigDictionaryTransaction4.ContainsValue(value))
            {
                return true;
            }
            if (_bigDictionaryTransaction5.ContainsValue(value))
            {
                return true;
            }
            if (_bigDictionaryTransaction6.ContainsValue(value))
            {
                return true;
            }
            if (_bigDictionaryTransaction7.ContainsValue(value))
            {
                return true;
            }
            if (_bigDictionaryTransaction8.ContainsValue(value))
            {
                return true;
            }
            if (_bigDictionaryTransaction9.ContainsValue(value))
            {
                return true;
            }
            if (_bigDictionaryTransaction10.ContainsValue(value))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check on every dictionary the id existance.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ContainsKey(long id)
        {
            if (_bigDictionaryTransaction1.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction2.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction3.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction4.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction5.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction6.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction7.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction8.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction9.ContainsKey(id))
            {
                return true;
            }
            if (_bigDictionaryTransaction10.ContainsKey(id))
            {
                return true;
            }
            return false;
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

        }

        /// <summary>
        /// Return values of dictionary.
        /// </summary>
        /// <returns></returns>
        public string Values()
        {
            return string.Join(string.Empty, _bigDictionaryTransaction1.Values) +
                string.Join(string.Empty, _bigDictionaryTransaction2.Values) +
                string.Join(string.Empty, _bigDictionaryTransaction3.Values) +
                string.Join(string.Empty, _bigDictionaryTransaction4.Values) +
                string.Join(string.Empty, _bigDictionaryTransaction5.Values) +
                string.Join(string.Empty, _bigDictionaryTransaction6.Values) +
                string.Join(string.Empty, _bigDictionaryTransaction7.Values) +
                string.Join(string.Empty, _bigDictionaryTransaction8.Values) +
                string.Join(string.Empty, _bigDictionaryTransaction9.Values) +
                string.Join(string.Empty, _bigDictionaryTransaction10.Values);
        }
    }
}