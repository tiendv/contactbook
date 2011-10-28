/// <reference path="jquery-1.4.1-vsdoc.js" />
/*
Copyright (c) <2010> Israel Cordeiro da Fonseca, http://www.metanoianet.com

 Permission is hereby granted, free of charge, to any person
 obtaining a copy of this software and associated documentation
 files (the "Software"), to deal in the Software without
 restriction, including without limitation the rights to use,
 copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the
 Software is furnished to do so, subject to the following
 conditions:

 The above copyright notice and this permission notice shall be
 included in all copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 OTHER DEALINGS IN THE SOFTWARE.

*/

jQRepeater = function(options)
{
    /// <summary>
    /// Generates an object of type repeater, i.e.  var rpt =  jQRepeater({rptClass:'container',headerClass:'header', itemClass:'item', footerClass:'footer'});
    /// </summary>
    /// <param name="rptClass">Class name of the container </param>
    /// <param name="headerClass">Class name of the header</param>
    /// <param name="itemClass">Class name of the itemTemplate</param>
    /// <param name="footerClass">Class name of the footer</param>
    /// <param name="validateRptType">Whether true or undefined the repeater will validate whether there jqrepeater the attribute with the value true. Unnecessary if you use the jQuery way "jQuery('#myContainer').jQRepeater('header', 'item', 'footer')"</param>    
    /// <param name="execContructor">Whether true or undefined the repeater will start the builder. Use if you need to change the header, footer or item template at runtime.</param>

    this.ValidateParameter = function(obj)
    {
        if (obj == undefined || obj == null || obj.toString().length == 0)
            return false;
        return true;
    }

    this.ValidateOptions = function(jsonOptions)
    {
        this.validatebyType = '';

        if (this.ValidateParameter(options.ValidateRptType) == false || options.ValidateRptType == true)
            this.validatebyType = '[jqrepeater=true]';

        if (typeof jsonOptions.RptClass === "string" && this.ValidateParameter(jsonOptions.RptClass))
        {
            this.Name = jsonOptions.RptClass;
            this.Container = jQuery('.' + this.Name + this.validatebyType);
        }
        else
        {
            this.Name = '';
            this.Container = jsonOptions.RptClass;
        }


        // header
        if (this.ValidateParameter(jsonOptions.HeaderClass))
        {
            this.headerClass = jsonOptions.HeaderClass;
            this.HeaderContainer = this.Container.find('.' + this.headerClass);
            if (this.ValidateParameter(jsonOptions.Header))
                this.Header = jsonOptions.Header;
            else
                this.Header = this.HeaderContainer.html();
        }
        //item
        if (this.ValidateParameter(jsonOptions.ItemClass))
        {
            this.itemClass = jsonOptions.ItemClass;
            this.ItemContainer = this.Container.find('.' + this.itemClass);
            if (this.ValidateParameter(jsonOptions.ItemTemplate))
                this.ItemTemplate = jsonOptions.ItemTemplate;
            else
                this.ItemTemplate = this.ItemContainer.html();
        }
        //footer
        if (this.ValidateParameter(jsonOptions.FooterClass))
        {
            this.footerClass = jsonOptions.FooterClass;
            this.FooterContainer = this.Container.find('.' + this.footerClass);
            if (this.ValidateParameter(jsonOptions.Footer))
                this.Footer = jsonOptions.Footer;
            else
                this.Footer = this.FooterContainer.html();
        }

        // tags
        if (this.ValidateParameter(jsonOptions.IndicatorIni))
            this.IndicatorIni = jsonOptions.IndicatorIni;
        else
            this.IndicatorIni = "(%";

        if (this.ValidateParameter(jsonOptions.IndicatorEnd))
            this.IndicatorEnd = jsonOptions.IndicatorEnd;
        else
            this.IndicatorEnd = "%)";

        
        if (this.ValidateParameter(jsonOptions.ShowHide))
            this.ShowHide = jsonOptions.ShowHide;
        else
            this.ShowHide = true;

        if (this.ValidateParameter(jsonOptions.ShowHideTime))
            this.ShowHideTime = jsonOptions.ShowHideTime;
        else
            this.ShowHideTime = 0;
        
    }
    this.ValidateOptions(options);

    this.BindTemplate = function(template, DataItem, CurrentIndex, TotalItems, DataList)
    {
        /// <summary>
        /// Bind the template with the content and returns the result
        /// </summary>
        /// <param name="template">Html template</param>
        /// <param name="DataItem">Item</param>
        /// <param name="CurrentIndex">Current Index of DataList</param>
        /// <param name="TotalItems">Total Items</param>
        /// <param name="DataList">Original DataList</param>

        if (template == undefined || template == null)
            return '';
        else
            template = unescape(template);

        if (DataItem == null)
            DataItem = '';

        var rt = '';
        var idx = 0;
        for (var i = 0; i < template.length; i++)
        {
            i = template.indexOf(this.IndicatorIni, i);
            if (i < 0)
            {
                rt += template.substring(idx, template.length);
                break;
            }
            rt += template.slice(idx, i);
            rt += eval(template.slice(i + this.IndicatorIni.length, template.indexOf(this.IndicatorEnd, i)));
            i = idx = (template.indexOf(this.IndicatorEnd, i) + this.IndicatorEnd.length);
        }
        return rt;
    }

    this.Bind = function(DataList)
    {
        /// <summary>
        /// Runs the repeater, as it exists in every template
        /// </summary>
        /// <param name="DataList">Array containing the items</param>
        this.DataList = DataList;
        var lenFor = DataList.length;
        var htmlItem = '';        
        this.CurrentIndex = 0;
        
        if (this.ShowHide == true)
            this.Container.hide(this.ShowHideTime);
            
        if (this.Header != undefined && this.Header.length > 0)
            this.HeaderContainer.html(this.BindTemplate(this.Header, null, 0, DataList.length, DataList));

        if (this.ItemTemplate != undefined && this.ItemTemplate.length > 0)
        {
            for (var i = 0; i < lenFor; i++)
            {
                this.CurrentIndex = i + 1;
                htmlItem += this.BindTemplate(this.ItemTemplate, DataList[i], i, DataList.length, DataList);
            }
        }
        this.ItemContainer.html(htmlItem);

        if (this.Footer != undefined && this.Footer.length > 0)
            this.FooterContainer.html(this.BindTemplate(this.Footer, null, 0, DataList.length, DataList));

        
        if (this.ShowHide == true)
            this.Container.show(this.ShowHideTime);
    }

    this.ReturnTemplate = function(templateClass, templateContainer)
    {
        /// <summary>
        /// Returns the contents of the template informed through classes
        /// </summary>

        return templateContainer.find('.' + templateClass).html();
    }

    this.Clear = function(clearAll)
    {
        /// <summary>
        /// Clears the contents of the repeater
        /// </summary>
        // be carefull with clearAll, you loose everything and need 
        if (clearAll)
            this.Container.text('');
        else
        {
            this.HeaderContainer.text('');
            this.ItemContainer.text('');
            this.FooterContainer.text('');
        }

        if (this.ShowHide == true)
            jQuery(this.Container).hide(this.ShowHideTime);

    }
    this.CreateLinks = function(subOpt)
    {
        var id = subOpt.Id;        
        var List = null;
        var totalItems = 0;
        var subItemName = null;

        if (subOpt.List == undefined)
            List = this.DataList;
        else
            List = subOpt.List;

        if (subOpt.TotalItems == undefined)
            totalItems = List.length;
        else
            totalItems = subOpt.TotalItems;

        var options = null;

        if (subOpt.Options == undefined)
            options = { HeaderClass: 'subheader', ItemClass: 'subitem', FooterClass: 'subfooter', IndicatorIni: '($', IndicatorEnd: '$)' };
        else
            options = subOpt.Options;

        if (subOpt.SubItemName != undefined)
            subItemName = subOpt.SubItemName;

        //var options = { headerClass: 'subheader', itemClass: 'subitem', footerClass: 'subfooter', IndicatorIni: '($', IndicatorEnd: '$)' };
        for (var index = 0; index < totalItems; index++)
        {
            var objLnk = jQuery('#' + id + index.toString());

            jQuery(objLnk).click(
                function()
                {
                    if (this.isChecked == undefined)
                        this.isChecked = false;

                    if (this.subrpt == undefined)
                    {

                        var idContainer = '#' + jQuery(this).attr('SubContainer');
                        this.subrpt = jQuery(idContainer).jQRepeater(options);
                    }

                    if (this.isChecked == false)
                    {
                        if (subItemName == null)
                            this.subrpt.Bind(List);
                        else
                        {
                            var idxList = jQuery(this).attr('Index');
                            var execEval = 'this.subrpt.Bind(List[' + idxList + '].' + subItemName + ');';
                            eval(execEval);
                        }
                        this.isChecked = true;
                    }
                    else
                    {
                        this.subrpt.Clear();
                        this.isChecked = false;
                    }

                    return false;
                });
        }
        return this;
    }

    return this;

}

jQuery.fn.extend(
{
    jQRepeater: function(options)
    {
        /// <summary>
        /// Generates an object of type repeater i.e. jQuery('#objId').jQRepeater({headerClass:'header',itemClass:'item',footerClass:'footer'});
        /// </summary>        
        /// <param name="headerClass">Class name of the header</param>
        /// <param name="itemClass">Class name of the itemTemplate</param>
        /// <param name="footerClass">Class name of the footer</param>
        if (options == undefined)
            options = {rptClass:this};
        else
            options.RptClass = this;
        return new jQRepeater(options);
    }
}
);
