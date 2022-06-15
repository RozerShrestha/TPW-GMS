<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="test3.aspx.cs" Inherits="TPW_GMS.test3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content">
        <div class="row">
            <div class="col-xs-12">
                <input id="filter" type="text" value="" name="" placeholder="Search here......" class="form-control" />
            </div>
        </div>
        <br />
        <div id="testLoadArea" class="row custom-row" >
        
        </div>
    </section>
    
    
    <script>
        $(document).ready(function () {
            loadData();
        });
        function loadData() {
            var first = 1;
            var last = "";
            var bsDate = "";
            var DateArray = [];
            var npDate = ['2077', '2078', '2079', '2080', '2081', '2082', '2083', '2084', '2085', '2086']
            $.each(npDate, (key, value) => {
                for (var j = 1; j <= 12; j++) {
                    //gives days 
                    last = NepaliFunctions.GetDaysInBsMonth(value, j)
                    bsDateFirst = `${value}/${j}/1`;
                    bsDateLast = `${value}/${j}/${last}`;
                    var engDateFirst = BSToAD(bsDateFirst);
                    var engDateLast = BSToAD(bsDateLast);
                    DateArray.push({
                        NepFirst: bsDateFirst,
                        EngFirst: engDateFirst,
                        NepLast: bsDateLast,
                        EngLast:engDateLast
                    })

                }
            })  
        }
       function BSToAD(bs) {
            let bsObj = NepaliFunctions.ConvertToDateObject(bs, "YYYY/MM/DD");
            let adObj = NepaliFunctions.BS2AD(bsObj);
            let ad = NepaliFunctions.ConvertDateFormat(adObj, "YYYY/MM/DD");
            return ad;
        }
    </script>
</asp:Content>
