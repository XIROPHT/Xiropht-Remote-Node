using System.Collections.Generic;

namespace Xiropht_RemoteNode.Object
{
    public class DictionaryBlockHash
    {
        private Dictionary<string, long> ListBlockHash;

        /// <summary>
        /// Constructor
        /// </summary>
        public DictionaryBlockHash()
        {
            ListBlockHash = new Dictionary<string, long>();
        }

        /// <summary>
        /// Insert a block hash.
        /// </summary>
        /// <param name="blockHash"></param>
        /// <param name="blockId"></param>
        public bool InsertBlockHash(string blockHash, long blockId)
        {
            try
            {
                if (!ListBlockHash.ContainsKey(blockHash))
                {
                    ListBlockHash.Add(blockHash, blockId);
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        /// <summary>
        /// Return block id from block hash
        /// </summary>
        /// <param name="blockHash"></param>
        /// <returns></returns>
        public long GetBlockIdFromHash(string blockHash)
        {
            if (ListBlockHash.ContainsKey(blockHash))
            {
                return ListBlockHash[blockHash];
            }
            return -1;
        }

        public void Clear()
        {
            ListBlockHash.Clear();
        }
    }
}
