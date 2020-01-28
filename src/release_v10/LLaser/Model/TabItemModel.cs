//********************************************************* 
// LLaser project - TabItemModel.cs
// Created at 2013-4-25
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using LLaser.Common;

namespace LLaser.Model
{
    public class TabItemModel : ModelBase
    {
        #region fields and properties

        private ModelBase _content;
        private string _header;

        public string Header
        {
            get { return _header; }
            set { SetProperty(ref _header, value); }
        }

        public ModelBase Content
        {
            get { return _content; }
            set { SetProperty(ref _content, value); }
        }

        #endregion
    }
}