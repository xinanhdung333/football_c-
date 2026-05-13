/**
 * HƯỚNG DẪN CẬP NHẬT MAINFORM ĐỂ TÍCH HỢP CÁC FORM MỚI
 * 
 * MainForm hiện tại đã có 3 tab: Fields, Services, Cart
 * 
 * CÓ 2 CÁCH TÍCH HỢP:
 * ═══════════════════════════════════════════════════════════════
 * 
 * CÁCH 1: THÊM TAB MỚI VÀO TABCONTROL
 * (Dễ nhất, giao diện thống nhất)
 * 
 * Các tab cần thêm:
 * - tabBookings - Lịch sử đặt sân
 * - tabFeedback - Phản hồi
 * - tabChatbot - Chatbot
 * 
 * ═══════════════════════════════════════════════════════════════
 * 
 * CÁCH 2: THÊM BUTTON/MENU MỞ CÁC FORM
 * (Sạch hơn, tách biệt logic)
 * 
 * Tạo panel menu ở trên/bên cạnh tabControl với các button:
 * - btnBookings
 * - btnFeedback
 * - btnChatbot
 * 
 * ═══════════════════════════════════════════════════════════════
 * 
 * ĐỂ CẬP NHẬT MAINFORM:
 * 
 * 1. Mở MainForm.Designer.cs
 * 2. Thêm các service vào MainForm.cs:
 * 
 *    private FeedbackService _feedbackService = null!;
 * 
 *    // Trong constructor
 *    public MainForm(..., FeedbackService feedbackService, ...)
 *    {
 *        _feedbackService = feedbackService;
 *    }
 * 
 * 3. Thêm các button event handler trong MainForm.cs:
 * 
 *    private void BtnBookings_Click(object sender, EventArgs e)
 *    {
 *        BookingsForm form = new BookingsForm(_bookingService, CurrentUser.UserId);
 *        form.ShowDialog();
 *    }
 * 
 *    private void BtnFeedback_Click(object sender, EventArgs e)
 *    {
 *        FeedbackForm form = new FeedbackForm(_feedbackService, CurrentUser.UserId);
 *        form.ShowDialog();
 *    }
 * 
 *    private void BtnChatbot_Click(object sender, EventArgs e)
 *    {
 *        ChatbotForm form = new ChatbotForm();
 *        form.ShowDialog();
 *    }
 * 
 * 4. Cập nhật Program.cs để thêm các service vào DI container:
 * 
 *    services.AddScoped<FeedbackService>();
 *    services.AddScoped<MainForm>();
 * 
 * ═══════════════════════════════════════════════════════════════
 * 
 * HƯỚNG DẪN CHI TIẾT CẬP NHẬT MAINFORM.DESIGNER.CS:
 * 
 * 1. Thêm khai báo các button mới:
 * 
 *    private Button btnBookings;
 *    private Button btnFeedback;
 *    private Button btnChatbot;
 *    private Panel pnlMenu;
 * 
 * 2. Trong InitializeComponent(), thêm:
 * 
 *    pnlMenu = new Panel();
 *    btnBookings = new Button();
 *    btnFeedback = new Button();
 *    btnChatbot = new Button();
 *    
 *    // Panel menu
 *    pnlMenu.Location = new Point(12, 12);
 *    pnlMenu.Size = new Size(776, 50);
 *    pnlMenu.BorderStyle = BorderStyle.FixedSingle;
 *    
 *    // Button Bookings
 *    btnBookings.Location = new Point(10, 10);
 *    btnBookings.Size = new Size(100, 30);
 *    btnBookings.Text = "Lịch sử đặt sân";
 *    btnBookings.Click += BtnBookings_Click;
 *    pnlMenu.Controls.Add(btnBookings);
 *    
 *    // Button Feedback
 *    btnFeedback.Location = new Point(120, 10);
 *    btnFeedback.Size = new Size(100, 30);
 *    btnFeedback.Text = "Phản hồi";
 *    btnFeedback.Click += BtnFeedback_Click;
 *    pnlMenu.Controls.Add(btnFeedback);
 *    
 *    // Button Chatbot
 *    btnChatbot.Location = new Point(230, 10);
 *    btnChatbot.Size = new Size(100, 30);
 *    btnChatbot.Text = "Chatbot";
 *    btnChatbot.Click += BtnChatbot_Click;
 *    pnlMenu.Controls.Add(btnChatbot);
 *    
 *    this.Controls.Add(pnlMenu);
 * 
 * 3. Cập nhật vị trí tabControl:
 * 
 *    tabControl.Location = new Point(12, 70);  // Thay từ 12 thành 70
 * 
 * ═══════════════════════════════════════════════════════════════
 * 
 * LƯU Ý QUAN TRỌNG:
 * 
 * - FeedbackService cần được thêm vào Program.cs
 * - CurrentUser.UserId là static property lưu ID người dùng đang đăng nhập
 * - Các form sử dụng ShowDialog() để hiển thị như dialog modal
 * - Cần import các form mới: using FootballBooking.UI;
 */
