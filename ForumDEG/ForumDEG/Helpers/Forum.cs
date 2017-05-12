﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ForumDEG;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace ForumDEG.Helpers {
    class Forum {
        private HttpClient _client;

        public Forum() {
            _client = new HttpClient();
            _client.MaxResponseContentBufferSize = 256000;
        }

        public async Task<Models.Forum> GetForumAsync(string id) {
            Debug.WriteLine("Inside getForumAsync");
            var uri = new Uri(string.Format(Constants.RestUrl, "forums/" + id));

            try {
                var response = await _client.GetAsync(uri);
                Debug.WriteLine("[Forum API] got response");
                if (response.IsSuccessStatusCode) {

                    var content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("[Forum API]: " + content);

                    var obj = JObject.Parse(content);

                    string title = obj["theme"].ToString();
                    string place = obj["place"].ToString();
                    string schedules = obj["schedules"].ToString();
                    DateTime date = obj["date"].ToObject<DateTime>();
                    int seconds = obj["hour"].ToObject<int>();
                    TimeSpan hour = TimeSpan.FromSeconds(seconds);
                    string remoteId = obj["id"].ToString();

                    Debug.WriteLine("[Forum API]: Forum theme:" + title);
                    Debug.WriteLine("[Forum API]: Forum place:" + place);
                    Debug.WriteLine("[Forum API]: Forum schedules:" + schedules);
                    Debug.WriteLine("[Forum API]: Forum date:" + date.ToString());
                    Debug.WriteLine("[Forum API]: Forum hour:" + hour.ToString(@"hh\:mm\:ss"));

                    Models.Forum forum = new Models.Forum {
                        Title = title,
                        Place = place,
                        Schedules = schedules,
                        Date = date,
                        Hour = hour,
                        RemoteId = remoteId
                    };

                    return forum;
                }
                return null;
            } catch (Exception ex) {
                Debug.WriteLine("[Forum API exception]:" + ex.Message);
                return null;
            }
        }

        public async Task<List<Models.Forum>> GetForumsAsync() {
            var uri = new Uri(string.Format(Constants.RestUrl, "forums"));

            try {
                var response = await _client.GetAsync(uri);
                List<Models.Forum> forums = new List<Models.Forum>();

                if (response.IsSuccessStatusCode) {
                    var content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("[Forum API] - Forums: " + content);

                    var objArray = JArray.Parse(content);

                    foreach (JObject obj in objArray) {
                        string title = obj["theme"].ToString();
                        string place = obj["place"].ToString();
                        string schedules = obj["schedules"].ToString();
                        DateTime date = obj["date"].ToObject<DateTime>();
                        int seconds = obj["hour"].ToObject<int>();
                        TimeSpan hour = TimeSpan.FromSeconds(seconds);
                        string remoteId = obj["id"].ToString();

                        Debug.WriteLine("[Forum API]: Forum theme:" + title);
                        Debug.WriteLine("[Forum API]: Forum place:" + place);
                        Debug.WriteLine("[Forum API]: Forum schedules:" + schedules);
                        Debug.WriteLine("[Forum API]: Forum date:" + date.ToString());
                        Debug.WriteLine("[Forum API]: Forum hour:" + hour.ToString(@"hh\:mm\:ss"));

                        forums.Add(new Models.Forum {
                            Title = title,
                            Place = place,
                            Schedules = schedules,
                            Date = date,
                            Hour = hour,
                            RemoteId = remoteId
                        });
                    }
                }
                return forums;
            } catch (Exception ex) {
                Debug.WriteLine("[Forum API exception]:" + ex.Message);
                return null;
            }
        }

        public async Task<bool> PostForumAsync(Models.Forum forum) {
            var uri = new Uri(string.Format(Constants.RestUrl, "forums"));

            var theme = forum.Title;
            var place = forum.Place;
            var schedules = forum.Schedules;
            var date = forum.Date;
            var hour = forum.Hour.TotalSeconds;

            var forumContents = new JObject();
            forumContents.Add("theme", theme);
            forumContents.Add("place", place);
            forumContents.Add("schedules", schedules);
            forumContents.Add("date", date);
            forumContents.Add("hour", hour);

            var body = new JObject();
            body.Add("forum", forumContents);

            Debug.WriteLine("[Forum API] - preparing to build content");

            var content = new StringContent(body.ToString(), Encoding.UTF8, "application/json");
            var contentString = await content.ReadAsStringAsync();
            Debug.WriteLine("[Forum API] - content built: " + contentString);

            try {
                var response = await _client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode) {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("[Forum API] - Post result: " + responseContent);
                    return true;
                } else {
                    var failedContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("[Forum API] - Post response unsuccessful " + failedContent);
                    return false;
                }
            } catch (Exception ex) {
                Debug.WriteLine("[Forum API exception]:" + ex.Message);
                return false;
            }
        }
    }
}