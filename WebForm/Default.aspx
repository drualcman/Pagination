<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebForm._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <h2>Sample List</h2>
            <table class="table table-responsive">
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>Created</th>
                        <th>Name</th>
                        <th>Description</th>
                        <th>Price</th>
                        <th>Qty</th>
                    </tr>
                </thead>
                <%
                    foreach (WebForm.SampleClass item in SampleList)
                    {
                %>
                <tr>
                    <th><%= item.Id %></th>
                    <th><%= item.Created %></th>
                    <th><%= item.Name %></th>
                    <th><%= item.Description %></th>
                    <th><%= item.Price %></th>
                    <th><%= item.Qty %></th>
                </tr>
                <% } %>
                <tfoot>
                    <tr>
                        <td colspan="6">
                            <%= WebForm.PagedList<WebForm.SampleClass>.PagedListPager(SampleList) %>
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>

</asp:Content>
