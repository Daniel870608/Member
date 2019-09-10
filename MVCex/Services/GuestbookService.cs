using MVCex.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace MVCex.Services
{
    public class GuestbookService
    {
        private readonly string cnstr = ConfigurationManager.ConnectionStrings["Guestbook"].ConnectionString;

        #region Index 所有資料
        /*public List<Gbook> GetAllGuestbooks(ForPaging forPaging, string Search = "")
        {
            List<Gbook> result = new List<Gbook>();
            string sql_AllData = "SELECT * FROM Guestbook";
            using (SqlConnection conn = new SqlConnection(cnstr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql_AllData, conn))
                {
                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        Gbook gbook = new Gbook();
                        gbook.Id = Convert.ToInt32(rd[0]);
                        gbook.Name = rd[1].ToString();
                        gbook.Content = rd[2].ToString();
                        gbook.CreateTime = Convert.ToDateTime(rd[3]);
                        gbook.Reply = rd[4].ToString();
                        if (!Convert.IsDBNull(rd[5]))
                        {
                            gbook.ReplyTime = Convert.ToDateTime(rd[5]);
                        }
                        result.Add(gbook);
                    }
                    rd.Close();
                }
                conn.Close();
            }
            return result;
        }*/
        #endregion

        #region Index 加入搜尋
        /*public List<Gbook> GetAllGuestbooks(ForPaging forPaging, string Search = "")
        {
            List<Gbook> result = new List<Gbook>();
            //字串過長，使用StringBuilder
            StringBuilder sql = new StringBuilder();
            //搜尋 -> WHERE 條件   ||   分頁  -> ROW_NUMBER() OVER(ORDER BY Id) as Sort
            sql.Append(@"SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY Id) as Sort,* FROM Guestbook WHERE Name LIKE '%' + '" + Search + "' + '%' or Content LIKE '%' + '" + Search + "' + '%' or Reply LIKE '%' + '" + Search + "' + '%' ) AS Guestbook ");
            using (SqlConnection conn = new SqlConnection(cnstr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql.ToString(), conn))
                {
                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        Gbook gbook = new Gbook();
                        gbook.Id = Convert.ToInt32(rd[0]);
                        gbook.Name = rd[1].ToString();
                        gbook.Content = rd[2].ToString();
                        gbook.CreateTime = Convert.ToDateTime(rd[3]);
                        gbook.Reply = rd[4].ToString();
                        if (!Convert.IsDBNull(rd[5]))
                        {
                            gbook.ReplyTime = Convert.ToDateTime(rd[5]);
                        }
                        result.Add(gbook);
                    }
                    rd.Close();
                }
                conn.Close();
            }
            return result;
        }*/
        #endregion

        #region Index 加入搜尋和分頁
        public List<Gbook> GetAllGuestbooks(ForPaging forPaging, string Search = "")
        {
            List<Gbook> result = new List<Gbook>();
            //字串過長，使用StringBuilder
            StringBuilder sql = new StringBuilder();
            //搜尋 -> WHERE 條件   ||   分頁  -> ROW_NUMBER() OVER(ORDER BY Id) as Sort
            sql.Append(@"SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY Id) as Sort,* FROM Guestbook WHERE Name LIKE '%' + '" + Search + "' + '%' or Content LIKE '%' + '" + Search + "' + '%' or Reply LIKE '%' + '" + Search + "' + '%' ) AS Guestbook ");
            sql.Append(@"WHERE Guestbook.Sort BETWEEN " + ((forPaging.NowPage - 1) * forPaging.ItemNum + 1) + " AND " + forPaging.NowPage * forPaging.ItemNum);
            string sql_AllData = "SELECT * FROM Guestbook";
            using (SqlConnection conn = new SqlConnection(cnstr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql.ToString(), conn))
                {
                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        Gbook gbook = new Gbook();
                        gbook.Id = Convert.ToInt32(rd[1]);
                        gbook.Name = rd[2].ToString();
                        gbook.Content = rd[3].ToString();
                        gbook.CreateTime = Convert.ToDateTime(rd[4]);
                        gbook.Reply = rd[5].ToString();
                        if (!Convert.IsDBNull(rd[6]))
                        {
                            gbook.ReplyTime = Convert.ToDateTime(rd[6]);
                        }
                        result.Add(gbook);
                    }
                    rd.Close();
                }
                //計算列數
                int RowCount = 0;
                using (SqlCommand cmd = new SqlCommand(sql_AllData, conn))
                {
                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        RowCount++;
                    }
                    rd.Close();
                }
                forPaging.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(RowCount) / forPaging.ItemNum));
                forPaging.SetRightPage();
                conn.Close();
            }
            return result;
        }
        #endregion

        public void InsertNewGuestbook(Gbook Data)
        {
            Data.CreateTime = DateTime.Now;
            string sql = "INSERT INTO Guestbook(Name,Content,CreateTime) VALUES ('" + Data.Name + "','" + Data.Content + "','" + Data.CreateTime.ToString("yyyy/MM/dd HH:mm:ss") + "')";
            using (SqlConnection conn = new SqlConnection(cnstr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }
        


        public Gbook GetDataById(int Id)
        {
            Gbook SearchData = new Gbook();
            string sql = "SELECT * FROM Guestbook WHERE Id=" + Id;
            using (SqlConnection conn = new SqlConnection(cnstr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        SearchData.Id = Convert.ToInt32(rd[0]);
                        SearchData.Name = rd[1].ToString();
                        SearchData.Content = rd[2].ToString();
                        SearchData.CreateTime = Convert.ToDateTime(rd[3]);
                        SearchData.Reply = rd[4].ToString();
                        if (!string.IsNullOrEmpty(rd[5].ToString()))
                        {
                            SearchData.ReplyTime = Convert.ToDateTime(rd[5]);
                        }
                    }
                }
            }

            return SearchData;
        }

        public void EditGuestbook(Gbook EditData)
        {
            if (string.IsNullOrEmpty(EditData.Reply))
            {
                string sql = "UPDATE Guestbook SET Name='" + EditData.Name + "',Content='" + EditData.Content + "' WHERE Id=" + EditData.Id;
                using (SqlConnection conn = new SqlConnection(cnstr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
            }
            else
            {
                return;
            }
        }

        public void ReplyGuestbook(Gbook ReplyData)
        {
            if (!string.IsNullOrEmpty(ReplyData.Reply))
            {
                ReplyData.ReplyTime = DateTime.Now;
                string sql = "UPDATE Guestbook SET Reply='" + ReplyData.Reply + "',ReplyTime ='" + ReplyData.ReplyTime.ToString("yyyy/MM/dd HH:mm:ss") + "' WHERE Id=" + ReplyData.Id;
                using (SqlConnection conn = new SqlConnection(cnstr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
            }
            else
            {
                return;
            }
        }


        public void DeleteGuestbook(int id)
        {
            string sql = "DELETE FROM Guestbook WHERE Id=" + id;
            using (SqlConnection conn = new SqlConnection(cnstr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }
    }
}