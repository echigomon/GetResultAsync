using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GetkenAsync;
using ChkNamespaceAsync;
using ChkClassAsync;
using ChkFuncAsync;
using LRSkipAsync;

namespace GetResultAsync
{
    public class CS_GetResultAsync
    {
        #region 共有領域
        private CS_GetkenAsync          getken;
        private CS_ChkNamespaceAsync    chknamespace;
        private CS_ChkClassAsync        chkclass;
        private CS_ChkFuncAsync         chkfunc;
        private CS_LRskipAsync          lrskip;

        private static String _wbuf;
        private static Boolean _empty;
        public String Wbuf
        {
            get
            {
                return (_wbuf);
            }
            set
            {
                _wbuf = value;
                if (_wbuf == null)
                {   // 設定情報は無し？
                    _empty = true;
                }
                else
                {   // 整形処理を行う
                    if (lrskip == null)
                    {   // 未定義？
                        lrskip = new CS_LRskipAsync();
                    }
                    lrskip.ExecAsync(_wbuf);
                    _wbuf = lrskip.Wbuf;

                    // 作業の為の下処理
                    if (_wbuf.Length == 0 || _wbuf == null)
                    {   // バッファー情報無し
                        // _wbuf = null;
                        _empty = true;
                    }
                    else
                    {
                        _empty = false;
                    }
                }
            }
        }
        private static String _result;     // [Namespace][Class][Function]ＬＢＬ情報
        public String Result
        {
            get
            {
                return (_result);
            }
            set
            {
                _result = value;
            }
        }
        private String[] _arry;         // トークン抽出情報
        private static int _wcnt;

        private static Boolean _Is_namespace;
        public Boolean Is_namespace
        {
            get
            {
                return (_Is_namespace);
            }
            set
            {
                _Is_namespace = value;
            }
        }
        private static Boolean _Is_class;
        public Boolean Is_class
        {
            get
            {
                return (_Is_class);
            }
            set
            {
                _Is_class = value;
            }
        }
        private static Boolean _Is_func;
        public Boolean Is_func
        {
            get
            {
                return (_Is_func);
            }
            set
            {
                _Is_func = value;
            }
        }

        private static int _lno;        // 行Ｎｏ．
        public int Lno
        {
            get
            {
                return (_lno);
            }
            set
            {
                _lno = value;
            }
        }
        #endregion

        #region コンストラクタ
        public CS_GetResultAsync()
        {   // コンストラクタ
            _wbuf = "";           // 設定情報なし
            _empty = true;
            _result = "";

            _Is_namespace = false;      // 評価フラグ：false
            _Is_class = false;
            _Is_func = false;

            getken = new CS_GetkenAsync();
            chknamespace = new CS_ChkNamespaceAsync();
            chkclass = new CS_ChkClassAsync();
            chkfunc = new CS_ChkFuncAsync();
        }
        #endregion

        #region モジュール
        public async Task ClearAsync()
        {   // 作業領域の初期化
            _wbuf = null;       // 設定情報無し
            _empty = true;

            _Is_namespace = false;  // [Namespace]フラグ：false
            _Is_class = false;      // [Class]フラグ：false
            _Is_func = false;       // [Function]フラグ：false
        }

        public async Task ExecAsync()
        {   // 評価
            if (!_empty)
            {   // バッファーに実装有り？
                _result = "";       // 出力情報をクリア
            }
        }
        #endregion

        #region サブ・モジュール
        #endregion
    }
}
