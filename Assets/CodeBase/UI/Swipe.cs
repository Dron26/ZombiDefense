// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
//
// public class swipe : MonoBehaviour
// {
//     public Color[] colors;
//     public GameObject scrollbar, imageContent;
//     private float scroll_pos = 0;
//     float[] pos;
//     private bool runIt = false;
//     private float time;
//     private Button takeTheBtn;
//     int btnNumber;
//     // Start is called before the first frame update
//     void Start()
//     {
//
//     }
//
//     // Update is called once per frame
//     void Update()
//     {
//         pos = new float[transform.childCount];
//         float distance = 1f / (pos.Length - 1f);
//
//         if (runIt)
//         {
//             GecisiDuzenle(distance, pos, takeTheBtn);
//             time += Time.deltaTime;
//
//             if (time > 1f)
//             {
//                 time = 0;
//                 runIt = false;
//             }
//         }
//
//         for (int i = 0; i < pos.Length; i++)
//         {
//             pos[i] = distance * i;
//         }
//
//         if (Input.GetMouseButton(0))
//         {
//             scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
//         }
//         else
//         {
//             for (int i = 0; i < pos.Length; i++)
//             {
//                 if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
//                 {
//                     scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
//                 }
//             }
//         }
//
//
//         for (int i = 0; i < pos.Length; i++)
//         {
//             if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
//             {
//                 Debug.LogWarning("Current Selected Level" + i);
//                 transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
//                 imageContent.transform.GetChild(i).localScale = Vector2.Lerp(imageContent.transform.GetChild(i).localScale, new Vector2(1.2f, 1.2f), 0.1f);
//                 imageContent.transform.GetChild(i).GetComponent<Image>().color = colors[1];
//                 for (int j = 0; j < pos.Length; j++)
//                 {
//                     if (j != i)
//                     {
//                         imageContent.transform.GetChild(j).GetComponent<Image>().color = colors[0];
//                         imageContent.transform.GetChild(j).localScale = Vector2.Lerp(imageContent.transform.GetChild(j).localScale, new Vector2(0.8f, 0.8f), 0.1f);
//                         transform.GetChild(j).localScale = Vector2.Lerp(transform.GetChild(j).localScale, new Vector2(0.8f, 0.8f), 0.1f);
//                     }
//                 }
//             }
//         }
//
//
//     }
//
//     private void GecisiDuzenle(float distance, float[] pos, Button btn)
//     {
//         // btnSayi = System.Int32.Parse(btn.transform.name);
//
//         for (int i = 0; i < pos.Length; i++)
//         {
//             if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
//             {
//                 scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[btnNumber], 1f * Time.deltaTime);
//
//             }
//         }
//
//         for (int i = 0; i < btn.transform.parent.transform.childCount; i++)
//         {
//             btn.transform.name = ".";
//         }
//
//     }
//     public void WhichBtnClicked(Button btn)
//     {
//         btn.transform.name = "clicked";
//         for (int i = 0; i < btn.transform.parent.transform.childCount; i++)
//         {
//             if (btn.transform.parent.transform.GetChild(i).transform.name == "clicked")
//             {
//                 btnNumber = i;
//                 takeTheBtn = btn;
//                 time = 0;
//                 scroll_pos = (pos[btnNumber]);
//                 runIt = true;
//             }
//         }
//
//        
//     }
//
// }

using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Swipe : MonoBehaviour
    {
        public Color[] colors;
        public GameObject scrollbar, imageContent;
        private float scroll_pos = 0;
        float[] pos;
        private bool runIt = false;
        private float time;
        private Button takeTheBtn;
        int btnNumber;

        private void Start()
        {
            pos = new float[transform.childCount];
            var buttonLeft = transform.Find("ButtonLeft").GetComponent<Button>();
            var buttonRight = transform.Find("ButtonRight").GetComponent<Button>();
            buttonLeft.onClick.AddListener(HandleButtonLeftClick);
            buttonRight.onClick.AddListener(HandleButtonRightClick);
        }


        public void HandleScrollBarValueChanged(float value)
        {
            HandleScrollBarPosition();
        }
        public void ButtonLeftClick()
        {
            if (btnNumber > 0)
            {
                btnNumber--;
                takeTheBtn = transform.GetChild(btnNumber).GetComponent<Button>();
                runIt = true;
            }
        }
        public void ButtonRightClick()
        {
            if (btnNumber < transform.childCount - 1)
            {
                btnNumber++;
                takeTheBtn = transform.GetChild(btnNumber).GetComponent<Button>();
                runIt = true;
            }
        }
        private void HandleButtonLeftClick()
        {
            ButtonLeftClick();
            HandleTransition();
        }

        private void HandleButtonRightClick()
        {
            ButtonRightClick();
            HandleTransition();
        }
        private void HandleTransition()
        {
            float distance = 1f / (pos.Length - 1f);

            if (runIt)
            {
                GecisiDuzenle(distance, pos, takeTheBtn);
                time += Time.deltaTime;

                if (time > 1f)
                {
                    time = 0;
                    runIt = false;
                }
            }
        }

        private void HandleScrollBarPosition()
        {
            float distance = 1f / (pos.Length - 1f);

            for (int i = 0; i < pos.Length; i++)
            {
                pos[i] = distance * i;
            }

            if (Input.GetMouseButton(0))
            {
                scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
            }
            else
            {
                for (int i = 0; i < pos.Length; i++)
                {
                    if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                    {
                        scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                    }
                }
            }
        }

        private void HandleSelectedLevel()
        {
            float distance = 1f / (pos.Length - 1f);

            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    Debug.LogWarning("Current Selected Level" + i);
                    transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
                    imageContent.transform.GetChild(i).localScale = Vector2.Lerp(imageContent.transform.GetChild(i).localScale, new Vector2(1.2f, 1.2f), 0.1f);
                    imageContent.transform.GetChild(i).GetComponent<Image>().color = colors[1];
                    for (int j = 0; j < pos.Length; j++)
                    {
                        if (j != i)
                        {
                            imageContent.transform.GetChild(j).GetComponent<Image>().color = colors[0];
                            imageContent.transform.GetChild(j).localScale = Vector2.Lerp(imageContent.transform.GetChild(j).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                            transform.GetChild(j).localScale = Vector2.Lerp(transform.GetChild(j).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                        }
                    }
                }
            }
        }

        private void GecisiDuzenle(float distance, float[] pos, Button btn)
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_pos < pos[i]

                    && scroll_pos > pos[i] - (distance / 2))
                {
                    if (btnNumber == i)
                    {
                        btn.onClick.Invoke();
                        runIt = false;
                    }
                    else
                    {
                        scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[btnNumber], 0.1f);
                        btnNumber = i;
                    }
                    break;
                }
            }
        }

        public void ButtonLeft()
        {
            if (btnNumber > 0)
            {
                btnNumber--;
                takeTheBtn = transform.GetChild(btnNumber).GetComponent<Button>();
                runIt = true;
            }
        }

        public void ButtonRight()
        {
            if (btnNumber < transform.childCount - 1)
            {
                btnNumber++;
                takeTheBtn = transform.GetChild(btnNumber).GetComponent<Button>();
                runIt = true;
            }
        }
    }
}