using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Async
{
    public class UIGameManager : MonoBehaviour
    {
        public static UIGameManager Instance {  get; private set; }
        private void Awake()
        {
            Instance = this;
        }
        public UIPanel[] UIPanels;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Switch<WeaponPanel>();
            }
        }
        public void Show<T>() where T : UIPanel
        {
            foreach(var  p in UIPanels)
            {
                if (p is T) p.Show();
            }
        }
        public void Hide<T>() where T : UIPanel
        {
            foreach (var p in UIPanels)
            {
                if (p is T) p.Hide();
            }
        }
        public void Switch<T> () where T : UIPanel
        {
            foreach (var p in UIPanels)
            {
                if (p is T) p.Switch();
            }
        }
    }
}