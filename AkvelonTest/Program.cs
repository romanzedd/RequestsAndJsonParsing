using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System.Text.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;



class User
{
    public int id { get; set; }
    public string username { get; set; }
    public string? about { get; set; }
    public int submitted { get; set; }
    public string? updated_at { get; set; }
    public int submission_count { get; set; }
    public int comment_count { get; set; }
    public int created_at { get; set; }

}

class ArticleUsers
{
    public int page { get; set; }
    public int per_page { get; set; }
    public int total { get; set; }
    public int total_pages { get; set; }
    public List<User>? data { get; set; }
}

class Result
{

    /*
     * Complete the 'getUsernames' function below.
     *
     * The function is expected to return a STRING_ARRAY.
     * The function accepts INTEGER threshold as parameter.
     *
     * URL for cut and paste
     * https://jsonmock.hackerrank.com/api/article_users?page=<pageNumber>
     */

    public static List<string> getUsernames(int threshold)
    {
        var data = new List<User>();

        var url = @"https://jsonmock.hackerrank.com/api/article_users?page=";
        var getRequset = WebRequest.Create(String.Concat(url, "1"));
        var stream = getRequset.GetResponse().GetResponseStream();
        var json = new StreamReader(stream).ReadToEnd();

        ArticleUsers? page = JsonSerializer.Deserialize<ArticleUsers>(json);
        if (page?.data != null) data.AddRange(page.data);
        
        for(int i = 2; i <= page.total_pages; i++)
        {
            getRequset = WebRequest.Create(String.Concat(url, i.ToString()));
            stream = getRequset.GetResponse().GetResponseStream();
            json = new StreamReader(stream).ReadToEnd();

            page = JsonSerializer.Deserialize<ArticleUsers>(json);
            if (page?.data != null) data.AddRange(page.data);
        }

        var result = new List<string>();
        result.AddRange(data.Where(x=> x.submission_count > threshold)
                         .Select(x => x.username).ToList());

        return result;
    }

}

class Solution
{
    public static void Main(string[] args)
    {
        Console.WriteLine(String.Join("\n", Result.getUsernames(10)));

    }
}
