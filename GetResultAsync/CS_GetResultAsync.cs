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
            _lno = 0;

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
            _lno = 0;

            _Is_namespace = false;  // [Namespace]フラグ：false
            _Is_class = false;      // [Class]フラグ：false
            _Is_func = false;       // [Function]フラグ：false
        }

        public async Task ExecAsync()
        {   // 評価
            if (!_empty)
            {   // バッファーに実装有り？
                _result = "";       // 出力情報をクリア
                await chknamespace.ClearAsync();
                await chkclass.ClearAsync();
                await chkfunc.ClearAsync();

                // トークン評価を行う
                await getken.ClearAsync();
                await getken.ExecAsync(_wbuf);
                if (getken.Wcnt != 0)
                {   // トークン有り？
                    _wcnt = getken.Wcnt;
                    _arry = new string[_wcnt];
                    for (int i = 0; i < _wcnt; i++)
                    {   // 全ての要素に対して処理を行う
                        _arry[i] = getken.Array[i];
                    }
                    for (int i = 0; i < _wcnt; i++)
                    {   // 全ての要素に対して処理を行う
                        // [Namespace]に対する処理を行う。
                        await chknamespace.ExecAsync(_lno, _wbuf);
                        if (chknamespace.Is_namespace)
                        {   // [Namespace]検出？
                            _Is_namespace = chknamespace.Is_namespace;      // [Namespace]フラグ：true
                            continue;       // 名称取り出し
                        }
                        else
                        {
                            if ((_Is_namespace) && (!string.IsNullOrEmpty(chknamespace.Result)))
                            {   // [Namespace]情報を抜き出したか？
                                _result = chknamespace.Result;
                                _empty = false;
                                _Is_namespace = false;
                                break;
                            }
                        }

                        // [Class]に対する処理を行う。
                        await chkclass.ExecAsync(_lno, _wbuf);
                        if (chkclass.Is_class)
                        {   // [Class]検出？
                            _Is_class = chkclass.Is_class;      // [Class]フラグ：true
                            continue;       // 名称取り出しへ
                        }
                        else
                        {
                            if ((_Is_class) && (!string.IsNullOrEmpty(chkclass.Result)))
                            {   // [Class]情報を抜き出したか？
                                _result = chkclass.Result;
                                _empty = false;
                                _Is_class = false;
                                break;
                            }
                        }

                        // [Function]に対する処理を行う。
                        await chkfunc.ExecAsync(_lno, _wbuf);
                        if (chkfunc.Is_func)
                        {   // [Function]検出？
                            _Is_func = chkfunc.Is_func;      // [Function]フラグ：true
                            // 名称取り出しへ
                        }
                        else
                        {
                            if ((_Is_func) && (!string.IsNullOrEmpty(chkfunc.Result)))
                            {   // [Function]情報を抜き出したか？
                                _result = chkfunc.Result;
                                _empty = false;
                                _Is_func = false;
                            }
                        }
                    }
                }
                else
                {
                    _result = "";       // 出力情報をクリア
                    _empty = true;
                }
            }
            else
            {
                _result = "";       // 出力情報をクリア
            }
        }
        #endregion

        #region サブ・モジュール
        #endregion
    }
}
