using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
  public static MenuManager Instance;
  public bool speedUpVid;

  [SerializeField] 
  Menu[] menus;

  [SerializeField]
  private TextMeshProUGUI versionText;


  private void Awake() {
    Instance = this;
    versionText.text = "Version: " + Application.version;
  }

  public void OpenMenu(string menuName) {
    for (int i = 0; i < menus.Length; i++) {
      if (menus[i].menuName == menuName) {
        menus[i].Open();
      } else if (menus[i].open) {
        menus[i].Close();
      }
    }
  }

  public void OpenMenu(Menu menu) {
    // First, close the currently open menu
    for (int i = 0; i < menus.Length; i++) {
      if (menus[i].open) {
        CloseMenu(menus[i]);
      }
    }
    menu.Open();
  }

  public void CloseMenu(Menu menu) {
    menu.Close();
  }

  public void SwitchSpeedUpVid(GameObject check) 
  {
      speedUpVid = !speedUpVid;
      check.gameObject.SetActive(speedUpVid);
  }

}
