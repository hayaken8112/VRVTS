using UnityEngine;
using UnityEngine.UI;

public class InputFieldFocus : MonoBehaviour {
    void Start() {
        InputField i = this.GetComponent<InputField>();
        i.ActivateInputField(); //InputFieldにフォーカスを持たせる
		/*
        i.onEndEdit.AddListener(
            delegate(string text) {
                if (!string.IsNullOrEmpty(text)) {
                    Debug.Log(text);    //textの送り先
                    i.text = "";
                }
                i.ActivateInputField(); //InputFieldにフォーカスを持たせる
            }
        );
		*/
        //4.6xではこの処理がないとEnterを押した後、InputFieldに改行が入ってしまう(Single Line設定でも)
        //5.2xでは要らないのでコメントアウト
//      i.onValidateInput += delegate(string _1, int _2, char c) { return c == '\n' ? '\0' : c; };
    }
}