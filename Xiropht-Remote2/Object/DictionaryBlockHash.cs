using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xiropht_RemoteNode.Object
{
    public class DictionaryBlockHash
    {
        private Dictionary<string, int> ListBlockHash;

        /// <summary>
        /// Constructor
        /// </summary>
        public DictionaryBlockHash()
        {
            ListBlockHash = new Dictionary<string, int>();
        }

        /// <summary>
        /// Insert a block hash.
        /// </summary>
        /// <param name="blockHash"></param>
        /// <param name="blockId"></param>
        public void InsertBlockHash(string blockHash, int blockId)
        {
            if (!ListBlockHash.ContainsKey(blockHash))
            {
                ListBlockHash.Add(blockHash, blockId);
            }
        }

        /// <summary>
        /// Return block id from block hash
        /// </summary>
        /// <param name="blockHash"></param>
        /// <returns></returns>
        public int GetBlockIdFromHash(string blockHash)
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
