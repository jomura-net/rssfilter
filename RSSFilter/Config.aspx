<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Config.aspx.cs" Inherits="RSSFilter.Config" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>RSS Filter -Config-</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblFeedURL" runat="server" Text="feed URL : "></asp:Label>
        <asp:TextBox ID="tbFeedURL" runat="server" Width="418px"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvFeedURL" runat="server" ControlToValidate="tbFeedURL"
            Display="Dynamic" ErrorMessage="RSS feedのURLを入力してください。"></asp:RequiredFieldValidator><br />
        <br />
        <asp:Label ID="lblFilterString" runat="server" Text="filter string : "></asp:Label>
        <asp:TextBox ID="tbFilterString" runat="server" Width="418px"></asp:TextBox><br />
        <br />
        <asp:Label ID="lblFilterTarget" runat="server" Text="filter target : "></asp:Label>&nbsp;
        <asp:CheckBox ID="cbTitle" runat="server" Checked="True" Text="Title" />
        &nbsp; &nbsp;
        <asp:CheckBox ID="cbDescription" runat="server" Text="Description" />
        &nbsp; &nbsp;<asp:CustomValidator ID="cvFilterTarget" runat="server" Display="Dynamic"
            ErrorMessage="CustomValidator" OnServerValidate="cvFilterTarget_ServerValidate"></asp:CustomValidator><br />
        <br />
        <asp:Button ID="btnGetFilterdFeed" runat="server" OnClick="btnGetFilterdFeed_Click"
            Text="Get Filterd Feed" /><br />
        <br />
        例1) Titleに、"PR:"という文字列が含まれるEntryを除外する場合、<br />
        filter string = "-PR:" &nbsp; ,&nbsp; filter target = [Title]<br />
        <br />
        例2) TitleもしくはDescriptionに、"仮想"が含まれ、"イメージ"が含まれないEntryだけを抽出する場合、<br />
        filter string = "仮想 -イメージ" &nbsp; ,&nbsp; filter target = [Title][Description]<br />
        <br />
        &nbsp;<asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="#CCCCCC"
            BorderStyle="None" BorderWidth="1px" CellPadding="3" Style="border-collapse: separate" AutoGenerateColumns="False"
            AllowPaging="true" PageSize="10" OnPageIndexChanging="GridView1_PageIndexChanging">
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <RowStyle ForeColor="#000066" />
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
            <Columns>
                <asp:HyperLinkField DataNavigateUrlFields="FeedURL" DataTextField="FeedURL" HeaderText="FeedURL" />
                <asp:HyperLinkField DataNavigateUrlFields="newFeedURL" DataTextField="FilterString" HeaderText="FilterString" />
                <asp:BoundField DataField="TargetTitle" HeaderText="Title" />
                <asp:BoundField DataField="TargetDescription" HeaderText="Desc." />
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
