using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Resource : MonoBehaviour
    {
        public Text Amount;
        public int Id;

        public void UpdateResource()
        {
            switch (Id)
            {
                case 0:
                    Amount.text = ResourceStorage.Stone.ToString();
                    break;
                case 1:
                    Amount.text = ResourceStorage.Iron.ToString();
                    break;
            }
        }
    }
}