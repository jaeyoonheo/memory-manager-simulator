using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Schema;

namespace Memory_Policy_Simulator
{
    class Core
    {
        private int cursor;
        public int p_frame_size;
        public List<Page> frame_window;
        public List<Page> pageHistory;
        public List<int> reference;
        public List<char> stack;

        public int hit;
        public int fault;
        public int migration;


        public Core(int get_frame_size)
        {
            this.cursor = 0;
            this.p_frame_size = get_frame_size;
            this.frame_window = new List<Page>();
            this.pageHistory = new List<Page>();
            this.reference = new List<int>();
            this.stack = new List<char>();
        }

        public Page.STATUS Operate(char data)
        {
            Page newPage = new Page();

            if (this.frame_window.Any<Page>(x => x.data == data))
            {
                newPage.pid = Page.CREATE_ID++;
                newPage.data = data;
                newPage.status = Page.STATUS.HIT;
                this.hit++;
                int i;

                for (i = 0; i < this.frame_window.Count; i++)
                {
                    if (this.frame_window.ElementAt(i).data == data) break;
                }
                newPage.loc = i + 1;
            }
            else
            {
                newPage.pid = Page.CREATE_ID++;
                newPage.data = data;

                if (frame_window.Count >= p_frame_size)
                {
                    newPage.status = Page.STATUS.MIGRATION;
                    newPage.migrate_page = cursor%p_frame_size;
                    this.frame_window.RemoveAt(cursor%p_frame_size);
                    cursor ++;
                    this.migration++;
                    this.fault++;
                }
                else
                {
                    newPage.status = Page.STATUS.PAGEFAULT;
                    cursor++;
                    this.fault++;
                }
                if (cursor > p_frame_size) cursor -= p_frame_size;
                newPage.loc = cursor;
                frame_window.Add(newPage);
            }
            pageHistory.Add(newPage);

            return newPage.status;
        }

        public Page.STATUS Optimal(char data,string Input,int num)
        {
            Page newPage = new Page();

            if (this.frame_window.Any<Page>(x => x.data == data))
            {
                newPage.pid = Page.CREATE_ID++;
                newPage.data = data;
                newPage.status = Page.STATUS.HIT;
                this.hit++;
                int i;

                for (i = 0; i < this.frame_window.Count; i++)
                {
                    if (this.frame_window.ElementAt(i).data == data) break;
                }
                newPage.loc = i + 1;
            }
            else
            {
                newPage.pid = Page.CREATE_ID++;
                newPage.data = data;

                if (frame_window.Count >= p_frame_size)
                {
                    List<int> distance = new List<int>();
                    int Ori_num = num;
                    for(int i=0; i<frame_window.Count;i++)
                    {
                        if (Ori_num < Input.Count()-1)
                        {
                            num++;
                            while(Input.ElementAt(num) != frame_window.ElementAt(i).data)
                            {
                                if (num >= Input.Count() - 1) { num++; break; }
                                num++;
                            }
                            distance.Add(num - Ori_num);
                            num = Ori_num;
                        }
                        else
                        {
                            distance.Add(Input.Count());
                        }
                    }
                    newPage.status = Page.STATUS.MIGRATION;
                    int max_index = distance.IndexOf(distance.Max());
                    newPage.migrate_page = max_index;
                    this.frame_window.RemoveAt(max_index);
                    cursor = p_frame_size;
                    this.migration++;
                    this.fault++;
                    newPage.loc = max_index + 1;
                    frame_window.Insert(max_index, newPage);
                }
                else
                {
                    newPage.status = Page.STATUS.PAGEFAULT;
                    cursor++;
                    this.fault++;
                    newPage.loc = cursor;
                    frame_window.Add(newPage);
                }
            }
            pageHistory.Add(newPage);

            return newPage.status;
        }

        public Page.STATUS LRU_CI(char data,int num)
        {
            Page newPage = new Page();

            if (this.frame_window.Any<Page>(x => x.data == data))
            {
                newPage.pid = Page.CREATE_ID++;
                newPage.data = data;
                newPage.status = Page.STATUS.HIT;
                this.hit++;
                int i;

                for (i = 0; i < this.frame_window.Count; i++)
                {
                    if (this.frame_window.ElementAt(i).data == data) break;
                }
                newPage.loc = i + 1;
                this.reference[i] = num;
            }
            else
            {
                newPage.pid = Page.CREATE_ID++;
                newPage.data = data;

                if (frame_window.Count >= p_frame_size)
                {
                    newPage.status = Page.STATUS.MIGRATION;
                    int min_index = this.reference.IndexOf(this.reference.Min());
                    newPage.migrate_page = min_index;
                    this.frame_window.RemoveAt(min_index);
                    cursor = p_frame_size;
                    this.migration++;
                    this.fault++;
                    newPage.loc = min_index + 1;
                    frame_window.Insert(min_index, newPage);
                    this.reference[min_index] = num;
                }
                else
                {
                    newPage.status = Page.STATUS.PAGEFAULT;
                    cursor++;
                    this.fault++;
                    this.reference.Add(num);
                    newPage.loc = cursor;
                    frame_window.Add(newPage);
                }
            }
            pageHistory.Add(newPage);

            return newPage.status;
        }

        public Page.STATUS LRU_SI(char data)
        {
            Page newPage = new Page();

            if (this.frame_window.Any<Page>(x => x.data == data))
            {
                newPage.pid = Page.CREATE_ID++;
                newPage.data = data;
                newPage.status = Page.STATUS.HIT;
                this.hit++;
                int i,j;

                for (i = 0; i < this.frame_window.Count; i++)
                {
                    if (this.frame_window.ElementAt(i).data == data) break;
                }
                newPage.loc = i + 1;
                for (j = 0; j < this.frame_window.Count; j++)
                {
                    if (this.frame_window.ElementAt(i).data == this.stack.ElementAt(j)) break;
                }
                this.stack.RemoveAt(j);
                this.stack.Add(data);
            }
            else
            {
                newPage.pid = Page.CREATE_ID++;
                newPage.data = data;

                if (frame_window.Count >= p_frame_size)
                {
                    int i = 0;
                    newPage.status = Page.STATUS.MIGRATION;
                    for (i = 0; i < this.frame_window.Count; i++)
                    {
                        if (this.frame_window.ElementAt(i).data == this.stack.ElementAt(0)) break;
                    }
                    int a = i;
                    if (i == this.frame_window.Count) i--;
                    newPage.migrate_page = i;
                    cursor = p_frame_size;
                    this.migration++;
                    this.fault++;
                    this.stack.RemoveAt(0);
                    this.frame_window.RemoveAt(i);
                    stack.Add(data);
                    newPage.loc = i + 1;
                    frame_window.Insert(i, newPage);
                }
                else
                {
                    newPage.status = Page.STATUS.PAGEFAULT;
                    cursor++;
                    this.fault++;
                    this.stack.Add(data);
                    newPage.loc = cursor;
                    frame_window.Add(newPage);
                }
            }
            pageHistory.Add(newPage);

            return newPage.status;
        }

        public Page.STATUS ARBA(char data)
        {
            Page newPage = new Page();

            for (int i = 0; i < frame_window.Count; i++)
            {
                this.reference[i] = this.reference[i] >> 1;
            }

            if (this.frame_window.Any<Page>(x => x.data == data))
            {
                newPage.pid = Page.CREATE_ID++;
                newPage.data = data;
                newPage.status = Page.STATUS.HIT;
                this.hit++;
                int i;

                for (i = 0; i < this.frame_window.Count; i++)
                {
                    if (this.frame_window.ElementAt(i).data == data) break;
                }
                newPage.loc = i + 1;

                this.reference[i] += 0b10000000;

            }
            else
            {
                newPage.pid = Page.CREATE_ID++;
                newPage.data = data;

                if (frame_window.Count >= p_frame_size)
                {
                    newPage.status = Page.STATUS.MIGRATION;
                    int min_index = this.reference.IndexOf(this.reference.Min());
                    newPage.migrate_page = min_index;
                    this.frame_window.RemoveAt(min_index);
                    cursor = p_frame_size;
                    this.migration++;
                    this.fault++;
                    newPage.loc = min_index + 1;
                    frame_window.Insert(min_index, newPage);
                    this.reference[min_index] = 0;
                }
                else
                {
                    newPage.status = Page.STATUS.PAGEFAULT;
                    cursor++;
                    this.fault++;
                    this.reference.Add(0);
                    newPage.loc = cursor;
                    frame_window.Add(newPage);
                }
            }
            pageHistory.Add(newPage);

            return newPage.status;
        }

        public Page.STATUS SCA(char data)
        {
            Page newPage = new Page();

            if (this.frame_window.Any<Page>(x => x.data == data))
            {
                newPage.pid = Page.CREATE_ID++;
                newPage.data = data;
                newPage.status = Page.STATUS.HIT;
                this.hit++;
                int i;

                for (i = 0; i < this.frame_window.Count; i++)
                {
                    if (this.frame_window.ElementAt(i).data == data) break;
                }
                newPage.loc = i + 1;

                this.reference[i] = 1;

            }
            else
            {
                newPage.pid = Page.CREATE_ID++;
                newPage.data = data;

                if (frame_window.Count >= p_frame_size)
                {
                    newPage.status = Page.STATUS.MIGRATION;
                    int i = cursor % p_frame_size;
                    while(this.reference.ElementAt(i)!=0)
                    {
                        this.reference[i] = 0;
                        if (i<=this.reference.Count)
                        {
                            i++;
                        }
                        if (i >= this.reference.Count)
                        {
                            i = 0;
                        }
                    }
                    this.cursor = i+1;
                    newPage.migrate_page = i;
                    this.frame_window.RemoveAt(i);
                    this.migration++;
                    this.fault++;
                    newPage.loc = i + 1;
                    frame_window.Insert(i, newPage);
                }
                else
                {
                    newPage.status = Page.STATUS.PAGEFAULT;
                    cursor++;
                    this.fault++;
                    this.reference.Add(0);
                    newPage.loc = cursor;
                    frame_window.Add(newPage);
                }
            }
            pageHistory.Add(newPage);

            return newPage.status;
        }

        public Page.STATUS NUR(char data)
        {
            Page newPage = new Page();
            List<int> temp = new List<int>();

            if (this.frame_window.Any<Page>(x => x.data == data))
            {
                newPage.pid = Page.CREATE_ID++;
                newPage.data = data;
                newPage.status = Page.STATUS.HIT;
                this.hit++;
                int i;

                for (i = 0; i < this.frame_window.Count; i++)
                {
                    if (this.frame_window.ElementAt(i).data == data) break;
                }
                newPage.loc = i + 1;

                if (reference[i] < 2) this.reference[i] += 2;

            }
            else
            {
                newPage.pid = Page.CREATE_ID++;
                newPage.data = data;

                if (frame_window.Count >= p_frame_size)
                {
                    int tp = 0;
                    tp = reference.Min();
                    newPage.status = Page.STATUS.MIGRATION;
                    int i = cursor % p_frame_size;
                    while (this.reference.ElementAt(i) !=tp)
                    {
                        if (reference.ElementAt(i) == 0) break;
                        else if (reference[i] % 2==1) reference[i] &= 10;
                        else if (reference[i] > 1) reference[i] -= 2;
                        
                        if (i <= this.reference.Count)
                        {
                            i++;
                        }
                        if (i >= this.reference.Count)
                        {
                            i = 0;
                        }
                    }
                    this.cursor = i + 1;
                    newPage.migrate_page = i;
                    this.frame_window.RemoveAt(i);
                    this.migration++;
                    this.fault++;
                    this.reference[i] = 1;
                    newPage.loc = i + 1;
                    frame_window.Insert(i, newPage);
                }
                else
                {
                    newPage.status = Page.STATUS.PAGEFAULT;
                    cursor++;
                    this.fault++;
                    this.reference.Add(0);
                    newPage.loc = cursor;
                    frame_window.Add(newPage);
                }
            }
            pageHistory.Add(newPage);

            return newPage.status;
        }

        public Page.STATUS LFU(char data)
        {
            Page newPage = new Page();

            if (this.frame_window.Any<Page>(x => x.data == data))
            {
                newPage.pid = Page.CREATE_ID++;
                newPage.data = data;
                newPage.status = Page.STATUS.HIT;
                this.hit++;
                int i;

                for (i = 0; i < this.frame_window.Count; i++)
                {
                    if (this.frame_window.ElementAt(i).data == data) break;
                }
                newPage.loc = i + 1;
                this.reference[i] += 1;
            }
            else
            {
                newPage.pid = Page.CREATE_ID++;
                newPage.data = data;

                if (frame_window.Count >= p_frame_size)
                {
                    newPage.status = Page.STATUS.MIGRATION;
                    int min_index = this.reference.IndexOf(this.reference.Min());
                    newPage.migrate_page = min_index;
                    this.frame_window.RemoveAt(min_index);
                    cursor = p_frame_size;
                    this.migration++;
                    this.fault++;
                    this.reference[min_index] = 1;
                    newPage.loc = min_index + 1;
                    frame_window.Insert(min_index, newPage);
                }
                else
                {
                    newPage.status = Page.STATUS.PAGEFAULT;
                    cursor++;
                    this.fault++;
                    this.reference.Add(1);
                    newPage.loc = cursor;
                    frame_window.Add(newPage);
                }
            }
            pageHistory.Add(newPage);

            return newPage.status;
        }

        public Page.STATUS MFU(char data)
        {
            Page newPage = new Page();

            if (this.frame_window.Any<Page>(x => x.data == data))
            {
                newPage.pid = Page.CREATE_ID++;
                newPage.data = data;
                newPage.status = Page.STATUS.HIT;
                this.hit++;
                int i;

                for (i = 0; i < this.frame_window.Count; i++)
                {
                    if (this.frame_window.ElementAt(i).data == data) break;
                }
                newPage.loc = i + 1;
                this.reference[i] += 1;
            }
            else
            {
                newPage.pid = Page.CREATE_ID++;
                newPage.data = data;

                if (frame_window.Count >= p_frame_size)
                {
                    newPage.status = Page.STATUS.MIGRATION;
                    int max_index = this.reference.IndexOf(this.reference.Max());
                    newPage.migrate_page = max_index;
                    this.frame_window.RemoveAt(max_index);
                    cursor = p_frame_size;
                    this.migration++;
                    this.fault++;
                    this.reference[max_index] = 1;
                    newPage.loc = max_index + 1;
                    frame_window.Insert(max_index, newPage);
                }
                else
                {
                    newPage.status = Page.STATUS.PAGEFAULT;
                    cursor++;
                    this.fault++;
                    this.reference.Add(1);
                    newPage.loc = cursor;
                    frame_window.Add(newPage);
                }
            }
            pageHistory.Add(newPage);

            return newPage.status;
        }

        public List<Page> GetPageInfo(Page.STATUS status)
        {
            List<Page> pages = new List<Page>();

            foreach (Page page in pageHistory)
            {
                if (page.status == status)
                {
                    pages.Add(page);
                }
            }

            return pages;
        }

    }


}