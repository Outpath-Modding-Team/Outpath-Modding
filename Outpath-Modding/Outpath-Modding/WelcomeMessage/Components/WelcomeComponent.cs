using UnityEngine;
using UnityEngine.UI;

namespace Outpath_Modding.WelcomeMessage.Components
{
    public class WelcomeComponent : MonoBehaviour
    {
        public Transform mainPanel { get; private set; }
        public Button closeButton { get; private set; }

        private void Awake()
        {
            mainPanel = transform.GetChild(1);
            closeButton = mainPanel.GetComponentInChildren<Button>();
            closeButton.onClick.AddListener(OnClickCloseButton);
            mainPanel.GetChild(1).gameObject.AddComponent<LinkOpener>();
        }

        public void OnClickCloseButton()
        {
            PlayerPrefs.SetInt("WelcomeMSG", 1);
            Destroy(gameObject);
        }
    }
}
