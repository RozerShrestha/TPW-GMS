<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Log.aspx.cs" Inherits="TPW_GMS.Log" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style>
        ol.list-decimal-numbering {
            list-style: none;
            counter-reset: decimal-counter;
        }

        ol.list-decimal-numbering li {
            counter-increment: decimal-counter;
            display: flex;
            align-items: baseline;
        }
        ol.list-decimal-numbering li {
            counter-increment: decimal-counter;
            display: flex;
            align-items: baseline;
        }
        .success{
            color:green;
        }
        .error{
            color:red;
        }
        .warn{
            color:brown
        }
        ol {
            margin-top: 30px;
        }
            /* below css is for numbering
  you can also give other style as well like
  text-color,background-color etc.
*/
            ol.list-decimal-numbering li::before {
                content: counter(decimal-counter) "-- ";
                font-size: 14px;
                font-weight: 700;
            }

       
    </style>
    <asp:HiddenField ID="hidHeader" runat="server" Value="Log Files Details" />
    <div id="app">
        <section>
            <div class="box box-info">
                <div class="box-body">
                    <div class="row">
                        <div class="col-sm-3">
                            <input type="text" v-model="dt" placeholder="by default todays date" id="txtDate" class="form-control" />
                        </div>
                        <div class="col-sm-1">
                            <%--<button type="button" v-on:click="getLogData" class="btn btn-info">Submit</button>--%>
                        </div>
                    </div>
                </div>
            </div>
            <ol class="mb-0 list-decimal-numbering">
                <li class="font-weight-bold finbyz-fadeinleft" v-for="(item, index) in log" :key="index">
                    <p class="font-weight-normal" v-bind:class="{success:item.First.search('INFO')>0, error:item.First.search('ERROR')>0,warn:item.First.search('WARN')>0}">
                         <strong>{{item.First}}</strong>
                        {{item.Second}}
                    </p>
                </li>
            </ol>   
        </section>
    </div>
    <script>
        new Vue({
            el: '#app',
            data: {
                log: [],
                dt: '',
            },
            methods: {
                getLogData() {
                    //const params = new URLSearchParams();
                    //params.append('date', this.formatDate(new Date()));
                    var dtt = this.dt == '' ? new Date() : new Date(this.dt);
                    axios({
                        url: 'api/GetLog',
                        method: 'post',
                        data: {
                            dt: this.formatDate(dtt)
                        },
                        dataType: "JSON",
                        headers: {
                            'Authorization': 'Bearer ' + window.localStorage.getItem('auth_token')
                        },
                    }).then(response => {
                        if (response.status == 200) {
                            this.log = response.data;
                        }
                        else {
                            console.log("Error");
                        }

                    }).catch(function (error) {
                        console.log(error)
                    })
                },
                formatDate(date) {
                    var d = new Date(date),
                        month = '' + (d.getMonth() + 1),
                        day = '' + d.getDate(),
                        year = d.getFullYear();

                    if (month.length < 2)
                        month = '0' + month;
                    if (day.length < 2)
                        day = '0' + day;
                    return [year, month, day].join('-');
                }
            },

            mounted: function () {
                var vm = this
                $('#txtDate').datepicker({
                    dateFormat: 'yy/mm/dd',
                    changeMonth: true,
                    changeYear: true,
                    onSelect: function (dateText) {
                        vm.dt = dateText
                    }
                })
                this.$nextTick(function () {
                    window.setInterval(() => {
                        this.getLogData();
                    }, 1000);
                })
            }
        })
    </script>

</asp:Content>
